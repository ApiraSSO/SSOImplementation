using System;
using DragonCMS.CMSSearchAdapter.Models.Directory.Person;
using DragonCMS.Directory.Messages.Models.V1;

namespace DragonCMS.ElasticSearchClientTests.MockDataFactories
{
    internal class PersonAggregateFactory
    {
        public static EsPersonSearch BuildPersonSearchModel(Guid personId, string foreName, string surname)
        {
            var model = new EsPersonSearch
            {
                Id = personId,
                PersonName = new PersonName { FirstName = foreName, LastName = surname }
            };
            return model;
        }

        public static void AddPersonOrganisation(EsPersonSearch personSearchModel, Guid organisationId, string organisationName)
        {
            personSearchModel.Organisations.Add(new CMSSearchAdapter.Models.Directory.Organisation.EsOrganisationSearch { Id = organisationId, OrganisationName = organisationName});
        }
    }
}
