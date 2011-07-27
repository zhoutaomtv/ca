/******************** ************************************************************************************************************************************************************************ 
目的：想把常用的js函数整一整，方便开发。希望同仁们一起来完善：）
版本：1。0
2009-01-21 by xwb
**********************************************************************************************************************************************************************************************/  
/********************  
函数名称：IsDate  函数功能：判断是否为date 函数参数：string date  返回值 是：true,否:false 
函数名称：CompareDate  函数功能：开始日期是否小于等于结束日期 函数参数：string startDate,string endDate 返回值 是：true,否:false
********************/ 



/********************  
函数名称：StrLenthByByte  
函数功能：计算字符串的字节长度，即英文算一个，中文算两个字节  
函数参数：str,为需要计算长度的字符串  
********************/  
function StrLenthByByte(str)  
{  
    var len;  
    var i;  
    len = 0;  
    for (i=0;i<str.length;i++)  
    {  
    if (str.charCodeAt(i)>255) len+=2; else len++;  
    }  
    return len;  
}  



/********************  
函数名称：IsEmailAddress  
函数功能：检查Email邮件地址的合法性，合法返回true,反之,返回false  
函数参数：obj,需要检查的Email邮件地址  
********************/  
function IsEmailAddress(obj)  
{  
    var pattern=/^[a-zA-Z0-9\-\_]+@[a-zA-Z0-9\-\.\_]+\.([a-zA-Z]{2,3})$/;  
    if(pattern.test(obj))  
    {  
    return true;  
    }  
    else  
    {  
    return false;  
}  
}  


/********************  
函数名称：IsTelephone  
函数功能：固话，手机号码检查函数，合法返回true,反之,返回false  
函数参数：obj,待检查的号码  
检查规则：  
　　(1)电话号码由数字、"("、")"和"-"构成  
　　(2)电话号码为3到8位  
　　(3)如果电话号码中包含有区号，那么区号为三位或四位  
　　(4)区号用"("、")"或"-"和其他部分隔开  
　　(5)移动电话号码为11或12位，如果为12位,那么第一位为0  
　　(6)11位移动电话号码的第一位和第二位为"13"  
　　(7)12位移动电话号码的第二位和第三位为"13"  
********************/  
function IsTelephone(obj)  
{  
    var pattern=/(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}1[0-9]{10}$)/;  
    if(pattern.test(obj))  
    {  
    return true;  
    }  
    else  
    {  
    return false;  
    }  
}  

/********************  
函数名称：IsLegality  
函数功能：检查字符串的合法性，即是否包含" ’字符，包含则返回false;反之返回true  
函数参数：obj,需要检测的字符串  
********************/  
function IsLegality(obj)  
{  
    var intCount1=obj.indexOf("\"",0);  
    var intCount2=obj.indexOf("\’",0);  
    if(intCount1>0 || intCount2>0)  
    {  
    return false;  
    }  
    else  
    {  
    return true;  
    }  
}  



/********************  
函数名称：Trim  
函数功能：去除字符串两边的空格  
函数参数：str,需要处理的字符串  
********************/  
function Trim(str)  
{  
    return str.replace(/(^\s*)|(\s*$)/g, "");  
}  
/********************  
函数名称：LTrim  
函数功能：去除左边的空格  
函数参数：str,需要处理的字符串  
********************/  
function LTrim(str)  
{  
    return str.replace(/(^\s*)/g, "");  
}  
/********************  
函数名称：RTrim  
函数功能：去除右边的空格  
函数参数：str,需要处理的字符串  
********************/  
function RTrim(str)  
{  
    return this.replace(/(\s*$)/g, "");  
}  
/********************  
函数名称：IsNull  
函数功能：判断给定字符串是否为空  
函数参数：str,需要处理的字符串  
********************/  
function IsNull(str)  
{  
    if(Trim(str)=="")  
    {  
    return true;  
    }  
    else  
    {  
    return false;  
}  
}  

/********************  
函数名称：CookieEnabled  
函数功能：判断cookie是否开启  
********************/  
function CookieEnabled()  
{  
    return (navigator.cookieEnabled)? true : false;  
}  

/********************************************************************  
**  
*函数功能:判断是否是闰年*  
*输入参数:数字字符串*  
*返回值:true，是闰年/false，其它*  
*调用函数:*  
**  
********************************************************************/  
function IsLeapYear(iYear)  
{  
    if (iYear+"" == "undefined" || iYear+""== "null" || iYear+"" == "")  
    return false;  
    iYear = parseInt(iYear);  
    varisValid= false;  
    if((iYear % 4 == 0 && iYear % 100 != 0) || iYear % 400 == 0)  
    isValid= true;  
    return isValid;    
}  

/********************************************************************  
**  
*
*取得页面请求*
* sample:Request.QueryString('complaintId')
*
**  
********************************************************************/  
var Request = { 
                QueryString : function( key )
                { 
                    var svalue = window.location.search.match(new RegExp( "[\?\&]" + key + "=([^\&]*)(\&?)", "i" )); 
                    return svalue ? svalue[1] : svalue; 
                } 
}; 


String.prototype.isNumeric = function(flag)  
{  
    //验证是否是数字  
    if(isNaN(this))  
    {  
    return false;  
    }  
    switch(flag)  
    {  
        case null://数字  
        case "":  
        return true;  
        case "+"://正数  
        return/(^\+?|^\d?)\d*\.?\d+$/.test(this);  
        case "-"://负数  
        return/^-\d*\.?\d+$/.test(this);  
        case "i"://整数  
        return/(^-?|^\+?|\d)\d+$/.test(this);  
        case "+i"://正整数  
        return/(^\d+$)|(^\+?\d+$)/.test(this);  
        case "-i"://负整数  
        return/^[-]\d+$/.test(this);  
        case "f"://浮点数  
        return/(^-?|^\+?|^\d?)\d*\.\d+$/.test(this);  
        case "+f"://正浮点数  
        return/(^\+?|^\d?)\d*\.\d+$/.test(this);  
        case "-f"://负浮点数  
        return/^[-]\d*\.\d$/.test(this);  
        default://缺省  
        return true;  
    }  
}  
/********************  
函数名称：IsNumber  
函数功能：检测字符串是否为各种数字类型
函数参数：str,需要检测的字符串 ,type,判断的类型
********************/  
function IsNumber(str,type)  
{  
    return str.isNumeric(type); 
}  

/********************  
函数名称：IsNonNegativeInteger  
函数功能：检测字符串是否为非负整数
函数参数：str,需要检测的字符串
********************/ 
function IsNonNegativeInteger( str ){
 var regu = /^(([1-9]\d*)|(0))$/;
 return regu.test(str);
}

/********************  
函数名称：IsZipCode  
函数功能：检测字符串是否为邮政编码
函数参数：str,需要检测的字符串 
********************/  
function IsZipCode(str)
{
    return /^[\d]{6}$/.test(str);  
}


/********************  
函数名称：IsIPAddress  
函数功能：检测字符串是否为IP地址
函数参数：str,需要检测的字符串 
********************/  
function IsIPAddress(str)
{
    if (reSpaceCheck.test(this))  
    {  
    this.match(reSpaceCheck);  
    if (RegExp.$1 <= 255 && RegExp.$1 >= 0  
     && RegExp.$2 <= 255 && RegExp.$2 >= 0  
     && RegExp.$3 <= 255 && RegExp.$3 >= 0  
     && RegExp.$4 <= 255 && RegExp.$4 >= 0)  
    {  
    return true;      
    }  
    else  
    {  
    return false;  
    }  
    }  
    else  
    {  
    return false;  
    }  
}

function $(obj){return document.getElementById(obj)}

/********************  
函数名称：trim  
函数功能：去除字符串两边的空格  
函数参数：  
********************/  
String.prototype.trim = function()
{
  return this.replace(/(^\s*)|(\s*$)/g, ''); 
} 

/********************  
函数名称：chked  
函数功能：checkbox  是否有勾选 
函数参数：divtbl
********************/  
function chked(divtbl)
{
    var checkboxs =  divtbl.getElementsByTagName('input');
    var chked = false;
    for (var i = 0; i<checkboxs.length; i++){
        if (checkboxs[i].name != 'chkAll' && checkboxs[i].checked)
            {chked=true; break;} 
    } 
    if (!chked)
        {alert('没有勾选任何选项');return false;}  
    else
        {return true;} 
}

/********************  
函数名称：IsDate  
函数功能：判断是否为date
函数参数：string date  
返回值 是：true,否:false
********************/  
function IsDate(strDate)
{
  var regStr = /^(\d{4})(\-)(\d{1,2})(\-)(\d{1,2})$/;
  return regStr.test(strDate);
} 

/********************  
函数名称：CompareDate  
函数功能：开始日期是否大于结束日期 
函数参数：string startDate,string endDate
返回值 是：true,否:false
********************/  
function CompareDate(start,end) 
{
   
   var arr=start.split("-");
   var starttime=new Date(arr[0],arr[1],arr[2]);
   var starttimes=starttime.getTime(); 
   var arrs=end.split("-"); 
   var endtime=new Date(arrs[0],arrs[1],arrs[2]);
   var endtimes=endtime.getTime();
   if(starttimes>endtimes)//开始大于结束
   {
     return false;
   } 
   else
   { 
    return true; 
   }
} 

/********************  
函数名称：replaceStr  
函数功能：字符串替换 
函数参数：源字串  替换字串 替换后的字串
返回值  Str
********************/  
function replaceStr(source, obj, target)
{
    var reg = new RegExp(obj,"g");
    return(source.replace(reg, target));
}


function IsHaveSpace(str)
{
    return /\s/.test(str);
}