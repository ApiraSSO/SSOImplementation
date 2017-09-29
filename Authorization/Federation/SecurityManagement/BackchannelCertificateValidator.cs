using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Kernel.Cryptography.Validation;
using SecurityManagement.BackchannelCertificateValidationRules;

namespace SecurityManagement
{
    internal class BackchannelCertificateValidator : IBackchannelCertificateValidator
    {
        private CertificateValidationConfiguration _configuration;

        private readonly ICertificateValidationConfigurationProvider _configurationProvider;
        public BackchannelCertificateValidator(ICertificateValidationConfigurationProvider configurationProvider)
        {
            if (configurationProvider == null)
                throw new ArgumentNullException("configurationProvider");

            this._configurationProvider = configurationProvider;
        }

        
        public bool Validate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            var federationPartyId = FederationPartyIdentifierHelper.GetFederationPartyIdFromRequestOrDefault(sender as HttpWebRequest);
            var configiration = this.GetConfiguration(federationPartyId);
            //ToDo: complete pinning validation. Moved to back log on 27/09/2017
            if(configiration.UsePinningValidation && configiration.BackchannelValidatorResolver != null)
            {
                try
                {
                    var type = configiration.BackchannelValidatorResolver.Type;
                    var instace = Activator.CreateInstance(type) as ICertificateValidatorResolver;
                    if(instace != null)
                    {
                        var validators = instace.Resolve();
                        
                        return true;
                    }
                }
                catch(Exception)
                {
                    return true;
                }
            }

            var context = new BackchannelCertificateValidationContext(certificate, chain, sslPolicyErrors);

            //default rule. No validation
            Func<BackchannelCertificateValidationContext, Task> seed = x =>
            {
                x.Validated();
                return Task.CompletedTask;
            };

            var rules = BackchannelCertificateValidationRulesFactory.GetRules(configiration);
            var validationDelegate = rules.Aggregate(seed, (f, next) => new Func<BackchannelCertificateValidationContext, Task>(c => next.Validate(c, f)));
            var task = validationDelegate(context);
            task.Wait();
            return context.IsValid;
        }
        
        private CertificateValidationConfiguration GetConfiguration(string federationPartyId)
        {
            if (this._configuration == null)
            {
                this._configuration = this._configurationProvider.GetConfiguration(federationPartyId);
            }
            if (this._configuration == null)
                throw new InvalidOperationException("CertificateValidationConfiguration is null!");

            return this._configuration;
        }
    }
}