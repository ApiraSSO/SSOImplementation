using System;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.Linq;
using Kernel.Cryptography.CertificateManagement;
using Kernel.Federation.MetaData.Configuration;
using Kernel.Federation.MetaData.Configuration.Cryptography;
using Kernel.Federation.MetaData.Configuration.EndPoint;
using Kernel.Federation.MetaData.Configuration.EntityDescriptors;
using Kernel.Federation.MetaData.Configuration.Organisation;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider
{
    internal class MetadataHelper
    {
        public static EntityDesriptorConfiguration BuildEntityDesriptorConfiguration(EntityDescriptorSettings entityDescriptorSettings)
        {
            var federationId = String.Format("{0}_{1}", "flowz", Guid.NewGuid());
            var organisation = entityDescriptorSettings.IncludeOrganisationInfo ? MetadataHelper.BuidOrganisationConfiguration(entityDescriptorSettings.Organisation) : (OrganisationConfiguration)null;
            var entityDescriptorConfiguration = new EntityDesriptorConfiguration
            {
                CacheDuration = MetadataHelper.TimeSpanFromDatapartEntry(entityDescriptorSettings.CacheDuration),
                EntityId = entityDescriptorSettings.EntityId,
                Id = entityDescriptorSettings.FederationId,
                ValidUntil = entityDescriptorSettings.ValidUntil,
                Organisation = organisation,
            };
            var spDescriptor = MetadataHelper.BuildSPSSODescriptorConfiguration(entityDescriptorSettings.RoleDescriptors.OfType<SPDescriptorSettings>().Single(), organisation);
            entityDescriptorConfiguration.RoleDescriptors.Add(spDescriptor);
            return entityDescriptorConfiguration;
        }

        public static KeyDescriptorConfiguration BuildKeyDescriptorConfiguration(Certificate certificate)
        {
            var certificateContext = new X509CertificateContext
            {
                StoreName = certificate.CetrificateStore,
                ValidOnly = false,
                StoreLocation = certificate.StoreLocation
            };

            certificate.StoreSearchCriteria.Aggregate(certificateContext.SearchCriteria, (t, next) =>
            {
                t.Add(new CertificateSearchCriteria { SearchValue = next.SearchCriteria, SearchCriteriaType = next.SearchCriteriaType });
                return t;
            });

            var keyDescriptorConfiguration = new KeyDescriptorConfiguration
            {
                IsDefault = certificate.IsDefault,
                Use = certificate.Use,
                CertificateContext = certificateContext
            };
            return keyDescriptorConfiguration;
        }

        private static OrganisationConfiguration BuidOrganisationConfiguration(OrganisationSettings organisationSettings)
        {
            var orgConfiguration = new OrganisationConfiguration
            {
                OrganisationContacts = new ContactConfiguration()
            };

            if (organisationSettings != null)
            {
                organisationSettings.Names.Aggregate(orgConfiguration.Names, (t, next) =>
                {
                    t.Add(new LocalizedConfigurationEntry
                    {
                        Name = next.Name,
                        DisplayName = String.IsNullOrEmpty(next.DisplayName) ? next.Name : next.DisplayName,
                        Language = new CultureInfo(next.Language)
                    });
                    return t;
                });

                organisationSettings.Urls.Aggregate(orgConfiguration.Urls, (t, next) =>
                {
                    t.Add(new LocalizedUrlEntry { Url = new Uri(next.Name), Language = new CultureInfo(next.Language) });
                    return t;
                });
                organisationSettings.Contacts.Aggregate(orgConfiguration.OrganisationContacts.PersonContact, (t, next) =>
                {
                    var contact = new Kernel.Federation.MetaData.Configuration.Organisation.ContactPerson
                    {
                        ContactType =next.ContactType,
                        ForeName = next.Forename.Name,
                        SurName = next.Surname.Name,

                    };
                    next.Emails.Aggregate(contact.Emails, (t1, next1) =>
                    {
                        contact.Emails.Add(next1.Name);
                        return t1;
                    });

                    next.Emails.Aggregate(contact.PhoneNumbers, (t2, next2) =>
                    {
                        contact.PhoneNumbers.Add(next2.Name);
                        return t2;
                    });
                    
                    t.Add(contact);
                    return t;
                });
            }
            return orgConfiguration;
        }
        
        private static SPSSODescriptorConfiguration BuildSPSSODescriptorConfiguration(SPDescriptorSettings sPDescriptor, OrganisationConfiguration organisation)
        {
            var sPSSODescriptorConfiguration = new SPSSODescriptorConfiguration
            {
                WantAssertionsSigned = sPDescriptor.WantAssertionsSigned,
                ValidUntil = sPDescriptor.ValidUntil,
                Organisation = organisation,
                AuthenticationRequestsSigned = sPDescriptor.RequestSigned,
                CacheDuration = MetadataHelper.TimeSpanFromDatapartEntry(sPDescriptor.CacheDuration),
                RoleDescriptorType = typeof(ServiceProviderSingleSignOnDescriptor),
                ErrorUrl = new Uri(sPDescriptor.ErrorUrl)
            };

            sPDescriptor.NameIdFormats.Aggregate(sPSSODescriptorConfiguration, (c, next) =>
            {
                c.NameIdentifierFormats.Add(new Uri(next.Uri));
                return c;
            });

            //logout services
            sPDescriptor.LogoutServices.Aggregate(sPSSODescriptorConfiguration.SingleLogoutServices, (t, next) =>
            {
                t.Add(new EndPointConfiguration
                {
                    Binding = new Uri(next.Binding.Uri),
                    Location = new Uri(next.Url)
                });
                return t;
            });
            //supported protocols
            sPDescriptor.Protocols.Aggregate(sPSSODescriptorConfiguration.ProtocolSupported, (t, next) =>
            {
                t.Add(new Uri(next.Uri));
                return t;
            });

            //key descriptors
            
            sPDescriptor.Certificates.Aggregate(sPSSODescriptorConfiguration.KeyDescriptors, (t, next) =>
            {
                var keyDescriptorConfiguration = MetadataHelper.BuildKeyDescriptorConfiguration(next);
                t.Add(keyDescriptorConfiguration);
                return t;
            });

            //assertion service
            sPDescriptor.AssertionServices.Aggregate(sPSSODescriptorConfiguration.AssertionConsumerServices, (t, next) =>
            {
                var indexedEndPointConfiguration = new IndexedEndPointConfiguration
                {
                    Index = next.Index,
                    IsDefault = next.IsDefault,
                    Binding = new Uri(next.Binding.Uri),
                    Location = new Uri(next.Url)
                };
                t.Add(indexedEndPointConfiguration);
                return t;
            });

            return sPSSODescriptorConfiguration;
        }
        
        private static TimeSpan TimeSpanFromDatapartEntry(DatepartValue datepart)
        {
            switch(datepart.Datepart)
            {
                case Datapart.Second:
                    return TimeSpan.FromSeconds(datepart.Value);
                case Datapart.Minute:
                    return TimeSpan.FromMinutes(datepart.Value);
                case Datapart.Hour:
                    return TimeSpan.FromHours(datepart.Value);
                case Datapart.Day:
                    return TimeSpan.FromDays(datepart.Value);
                default:
                    throw new NotSupportedException(String.Format("data part not supported: {0}", datepart));
            }
        }
    }
}