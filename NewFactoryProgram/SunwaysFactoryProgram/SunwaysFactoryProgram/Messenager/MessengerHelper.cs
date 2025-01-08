using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunwaysFactoryProgram.Messenager
{
    // 界面显示信息message
    public class Messenge_MainViewInf_Event : PubSubEvent<List<string>>
    {
    }

    public class TestStatusChangeEvent : PubSubEvent<TestStatusChange>
    {
    }

    public class LogoutInitEvent : PubSubEvent
    { 
    }

    // 扫码框锁定

    public class TextFocusEvent : PubSubEvent
    {
        
    }

    // 操作日志消息
    public class RecordLogEvent : PubSubEvent<string>
    {
    }

    // 包装测试
    public class FunctionViewTestEvent : PubSubEvent<string>
    {
    }

    // 包装测试
    public class PackViewTestEvent : PubSubEvent<string>
    {
    }

    public class RefreshBurninModel : PubSubEvent
    {
        
    }


    public class TestStatusChange
    { 
        public int Status { get; set; }
        public string Text { get; set; }

        public bool IsEnable = true;  
    }
}
