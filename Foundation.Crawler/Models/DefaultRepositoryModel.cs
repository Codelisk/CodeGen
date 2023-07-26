using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.Crawler.Models
{
    public class DefaultRepositoryModel
    {
        public DefaultRepositoryModel(INamedTypeSymbol repo, IMethodSymbol get, IMethodSymbol getAll, IMethodSymbol save, IMethodSymbol delete)
        {
            Repo = repo;
            Get = get;
            GetAll = getAll;
            Save = save;
            Delete = delete;
        }

        public INamedTypeSymbol Repo { get; set; }
        public IMethodSymbol Get { get; set; }
        public IMethodSymbol GetAll { get; set; }
        public IMethodSymbol Save { get; set; }
        public IMethodSymbol Delete { get; set; }
    }
}
