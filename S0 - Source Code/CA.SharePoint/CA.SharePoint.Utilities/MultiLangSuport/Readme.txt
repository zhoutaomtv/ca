
CodeArt,SPMultiLanSupport SharePoint多语言支持

使用方法：

1）确保服务器安装了中英文语言包

2)将CodeArt.SharePoint.MultiLanSupport.dll添加到GAC

3）在网站中建立一个自定义列表，名称为:MultiLanConfig
(每一个要转换的网站都要添加这个列表，不只是跟站点，若这个列表不存在，则系统认为这个站点不需要转换)

如果是中文站点添加英文支持，添加列 Eng 
如果是英文站点添加中文支持，添加列 Chs 
如果是要添加对其他语言的转换，则以这种语言的code为栏名，如日文：ja-jp
原则就是：每一种语言一列，Title存放网站本身的语言文字。

3)修改应用程序的web.config,
在HttpModules节点最下面添加：
<add name="MultiLanSupportModule" type="CodeArt.SharePoint.MultiLanSupport.MultiLanSupportModule,CodeArt.SharePoint.MultiLanSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fb342a992e9c6c52"/>
在<SafeControls>节点下面添加：

<SafeControl Assembly="CodeArt.SharePoint.MultiLanSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fb342a992e9c6c52" Namespace="CodeArt.SharePoint.MultiLanSupport" TypeName="*" Safe="True" />
  
4)默认情况，会按照用户IE的语言设置切换语言， 如果要允许用户直接通过站点控制，
需要将语言切换控件添加到站点的模板页：
添加：
<%@ Register Tagprefix="codeart" Namespace="CodeArt.SharePoint.MultiLanSupport" 
Assembly="CodeArt.SharePoint.MultiLanSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fb342a992e9c6c52" %>

在
<SharePoint:DelegateControl runat="server" ControlId="GlobalSiteLink0"/>
这个控件的前方插入：
<codeart:LanguageSwitch runat="server" ID="dd"/>
			  

----------------------------------------------------------
----------------------------------------------------------
MultiLanConfig列表使用方法：

在站点中任何要输入的地方(包括列表名，栏名，视图名，站点链接)都采用 $内容$ 的形式， $$之间的内容就是要被多语言处理的内容
在MultiLanConfig中添加翻译逻辑，如

中文站点翻译英文：
Title	Eng
联系人	Contact

英文文站点翻译中文：
Title	Chs
Contact	联系人 

中，英，日都支持则： 
Title	Eng		ja-jp
联系人	Contact	这儿写日文

5)对Layouts中的页面，系统不支持语言切换和翻译操作，

如果您切换Layouts中页面语言，则需要在每个页面中添加以下代码：

<%@Assembly Name="CodeArt.SharePoint.MultiLanSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fb342a992e9c6c52" %>
<script language="C#" runat="server">
    
    protected override void OnPreInit(EventArgs e)
    {
        CodeArt.SharePoint.MultiLanSupport.UICultureManager.CurrentInstance.SetThreadCulture();
        base.OnPreInit(e);
    }
</script>

这是因为未知原因，通过HttpModule中逻辑无法切换layous页面语言，即使通过以上代码“勉强”切换，大家会发现会出现切换了“一半”的情况，
因为Layouts页面可以看出系统页面，不进行多语言控制也说得过去。
后即版本可能对其进行支持。


6)快速启动，导航栏的翻译

也是一样的，不过大家要找到修改的地方：

网站操作-》快速启动
网站操作-》标题、说明和图标
网站操作-》顶部链接栏
网站操作-》导航

如果启用了“Office SharePoint Server 发布基础架构”功能，就换了个地方：
网站操作-》快速启动

在这些修改页面，将相应内容添加 $$标记，然后在MultiLanConfig中配置。

7）注意，代码中进行了缓存处理，所以修改了翻译配置后需要IISreset下才能看到效果