using System;
using System.Collections.Generic;

namespace DragonCMS.KafkaClient.Tests.MockData
{
    [Serializable]
    public class ParentTestClass
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public DateTimeOffset DateFiled { get; set; }
        public ICollection<string> TextCollection { get; set; }
        public int IntField { get; set; }

        public ICollection<ChildClass> Children { get; set; }

        public ChildClass Child { get; set; }

        public ParentTestClass()
        {
            this.Id = Guid.NewGuid();
            this.Children = new List<ChildClass>();
            this.TextCollection = new List<string>();
        }
    }
}