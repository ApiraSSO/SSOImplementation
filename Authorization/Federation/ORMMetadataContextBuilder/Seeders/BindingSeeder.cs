using Kernel.Data.ORM;
using Kernel.Federation.MetaData.Configuration;
using ORMMetadataContextProvider.Models;

namespace ORMMetadataContextProvider.Seeders
{
    internal class BindingSeeder : Seeder
    {
        public override void Seed(IDbContext context)
        {
            var redirectBinding = new Binding { Uri = Bindings.Http_Redirect, Name = "HTTP-Redirect" };
            context.Add<Binding>(redirectBinding);
            var postBinding = new Binding { Uri = Bindings.Http_Post, Name = "HTTP-POST" };
            context.Add<Binding>(postBinding);
            Seeder._cache.Add(Seeder.BindingsKey, new[] { redirectBinding, postBinding });
        }
    }
}