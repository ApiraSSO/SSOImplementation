using System;
using System.Collections.Generic;
using System.Linq;
using Data.Importing.Infrastructure;
using Data.Importing.Infrastructure.Validation;
using Data.Importing.Infrastructure.Validation.Rules;
using Kernel.DependancyResolver;
using Kernel.Validation;

namespace Data.Importing.Validation
{
    internal class ImportValidator : IImportValidator, IValidator
    {
        private readonly IDependencyResolver _dependencyResolver;
        private readonly IValidationRuleFactory _ruleFactory;
        public ImportValidator(IDependencyResolver dependencyResolver, IValidationRuleFactory ruleFactory)
        {
            this._dependencyResolver = dependencyResolver;
            this._ruleFactory = ruleFactory;
        }
        public void Validate(ValidationContext context, ImportState state)
        {
            throw new NotImplementedException();
        }

        void IValidator.Validate(ValidationContext context)
        {
            throw new NotImplementedException();
        }

        protected virtual ICollection<IValidationRule> GetValidationRulesForState(ImportState state)
        {
            if (state == null)
                throw new ArgumentNullException("state");

            var type = typeof(ISateValidationRule<>).MakeGenericType(state.GetType());
            return this._ruleFactory.GetValidationRules<ImportState>(s => this._dependencyResolver.ResolveAll(type))
                .ToList();
        }
    }
}