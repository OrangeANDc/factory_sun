using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTool.Model;

namespace SystemTool.Messenager
{
    internal class MessenagerHelper
    {
    }

    public class RefreshDgViewEvent : PubSubEvent<List<ParaModel>>
    {
        
    }

    // 通知主界面读取线程暂停或继续
    public class ControlTaskStatus : PubSubEvent<bool>
    {
        
    }

    // 主界面读取线程暂停后通知 子界面状态
    public class NotifyTaskStatus : PubSubEvent<bool>
    {

    }
}
