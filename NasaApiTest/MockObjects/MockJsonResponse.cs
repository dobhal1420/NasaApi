using NasaApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasaApiTest.MockObjects
{
    public class MockJsonResponse
    {
        public static NasaDataModel Get()
        {
            return new NasaDataModel()
            {
                Collection = new Collection()
                {
                    Href = "http://something",
                    Items = new List<NasaLineItem>() {
                       new NasaLineItem(){Href="http://somthin1" },
                       new NasaLineItem(){Href="http://something2" }
                    }
                }
            };
        }
    }
}

