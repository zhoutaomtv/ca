using System;
using System.ComponentModel;
using System.Web.UI.Design;

namespace CA.Web
{
    /// <summary>
    /// ObjectMapper设计器

    /// </summary>
    public class ObjectMapperDesigner: ControlDesigner
    {
        private ObjectMapper _ObjectMapper;

        public ObjectMapperDesigner() { }

        // 摘要:
        //     获取设计时html
        public override string GetDesignTimeHtml()
        {
            return _ObjectMapper.ID;
        }
        //
        // 摘要:
        //     初始化

        //
        // 参数:
        //   component:
        public override void Initialize(IComponent component)
        {
            _ObjectMapper = component as ObjectMapper;

            base.Initialize(component);
        }
    }
}
