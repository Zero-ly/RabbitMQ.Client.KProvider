using System;

namespace K.Mq.BaseTests
{
    /// <summary>
    /// 用于测试发送对象信息
    /// </summary>
    [Serializable]
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
