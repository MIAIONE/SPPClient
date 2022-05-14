#SPPClient 一种基于HWID直接激活系统的工具
A tool based on HWID direct activation of the system

##支持（Support）
仅限Retail Channel（零售版），不支持server版本和kms
★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆
可自行clone实现，KMS,GLVK的代码里面只是被注释了
不过tka模式和AD激活由于特殊，并未实现相关slmgr功能
请有vbs功底的自行重写
★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆★☆

##技术细节（Technical details）
SPPClient 通过从底层slc.dll,setupapi.dll,hid.dll等底层api直接实现HWID的获取，不再验证当前激活状态
HWID类使用了HwidApi，HwidApi来自@laomms的二进制文件反编译，反编译工具使用de4dot反混淆，dnSpy诱骗调试释放dll，dotPeek进行反编译+修复

传统数字激活软件：利用gatherosstate在WIN7下获得免费升级许可漏洞，通过写SYSTEM\Tokens注册表值，并强行用AppLayers模拟WIN7RTM兼容模式运行，同时利用破解的slc.dll，破除hwid获取因未激活获取不到正确hwid的情况，综合起来形成门票，在此之前先安装系统SKU Update Key，通过该Key可实现任意系统版本转换，然后利用clipup.exe安装骗取的数字证书，最后slmgr -ato激活系统
sppclient：彻底破除HWID限制，随意生成许可证，重写slmgr在C#内部的实现，直接调用WMI安装Key，不再有因用process或cscript运行带来的运行过程时间不确定，结果难获取，变量多等问题

##法律声明（Legal Notices）
本程序仅供科学研究之用途，请在下载后72h内自行删除，本项目为合法的逆向工程，不涉及任何微软激活相关代码
微软如有删除需要，请向我Github的邮箱发送邮件，将在7个工作日内完成；

##引用（References）
部分代码参考HwidApi，@laomms:gatherosstate C#实现

#下载（Download）
不再向Releases发布蓝奏云链接
改为此处：https://miaione.lanzoum.com/b09ba8dli
        密码:sppc
链接失效请提交issues，会及时补链接