using System;

namespace DragonCMS.KafkaClient.Tests.MockData
{
    [Serializable]
    public class ChildClass
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string ChildEmail { get; set; }
        public DateTimeOffset ChildDateFiled { get; set; }

        public int ChildIntField { get; set; }

        public ParentTestClass Parent { get; set; }
        public ChildClass()
        {
            this.Id = Guid.NewGuid();
            this.Name = "SomeName" + this.Id;
            this.ChildEmail = "email@domain.com";
        }
    }
}