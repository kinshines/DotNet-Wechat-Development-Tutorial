
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
}


//////////////////////////////////////

//将json数据转为对象
function ConvertDataTable(jsondata)
{
    return jsonStringToDataTable(jsondata);
}

//将json数据转为对象
function jsonStringToDataTable(jsondata){
    try{
    	var table=eval("("+jsondata+")");
		return table;
	}
	catch(ex){
	    return null ;
	}
}


//查詢參數對象( 可以 new querystring()创建 ).
function querystring()
{
	try
	{
      var url = unescape(window.location.href);
      var allargs = url.split("?")[1];
      var args = allargs.split("&");
      for(var i=0; i<args.length; i++)
      {
           var arg = args[i].split("=");
           eval('this.'+arg[0]+'="'+arg[1]+'";');
      }
    }
    catch(ex)
    {}
} 
// 提取返回URL中的参数,返回数组.
function getquerystring()
{
try
	{
       //加上substring的意义是去掉查询字符串中的？号。
       //var query = window.location.search.substring(1);
       //定义一个数组，用于存放取出来的字符串参数。
       var argsArr = new Object();
       //获取URL中的查询字符串参数
       var query = window.location.search;
       query = query.substring(1);
       //这里的pairs是一个字符串数组
       var pairs = query.split("&");//name=myname&password=1234&sex=male&address=nanjing
       for(var i=0;i<pairs.length;i++)
       {
			var sign = pairs[i].indexOf("="); 
			//如果没有找到=号，那么就跳过，跳到下一个字符串（下一个循环）。
			if(sign == -1)
			{
				continue; 
			}
			var aKey = pairs[i].substring(0,sign);
			var aValue = pairs[i].substring(sign+1);       
			argsArr[aKey] = aValue;
       }
       return argsArr;
    }
    catch(ex)
    {}
} 


//扩展String
String.prototype.trim = function() { 
    return this.replace(/(^\s*)|(\s*$)/g,"");
}
//
String.prototype.Replace=function (str1,str2){
    var rs=this.replace(new RegExp(str1,"gm"),str2);
    return rs;
}

////////////////////////////////
String.prototype.escape=function()
{
  return escape(this);
}
String.prototype.unescape=function()
{
  return unescape(this);
}

Date.prototype.format = function(format){
 var o = {
 "M+" : this.getMonth()+1, //month
 "d+" : this.getDate(),    //day
 "h+" : this.getHours(),   //hour
 "m+" : this.getMinutes(), //minute
 "s+" : this.getSeconds(), //second
 "q+" : Math.floor((this.getMonth()+3)/3),  //quarter
 "S" : this.getMilliseconds() //millisecond
 }
 if(/(y+)/.test(format)) format=format.replace(RegExp.$1,
 (this.getFullYear()+"").substr(4 - RegExp.$1.length));
 for(var k in o)if(new RegExp("("+ k +")").test(format))
 format = format.replace(RegExp.$1,
 RegExp.$1.length==1 ? o[k] :
 ("00"+ o[k]).substr((""+ o[k]).length));
 return format;
}

///////////////////////////

///////////////////////////////////////////

//可访问全局变量的Eval ,eval只能访问 局部变量
var Eval=function(code){
	if(!(window.attachEvent && !window.opera)){
		//ie
		execScript(code); 
	}
	else{
		//not ie
		window.eval(code);
	}
}

//实现一个Include
function Include(path,reload)
{
    var scripts = document.getElementsByTagName("script"); 
    if (!reload) 
    for (var i=0;i<scripts.length;i++) 
    if (scripts[i].src && scripts[i].src.toLowerCase() == path.toLowerCase() ) return; 
    var sobj = document.createElement('script'); 
    sobj.type = "text/javascript"; 
    sobj.src = path; 
    var headobj = document.getElementsByTagName('head')[0]; 
    headobj.appendChild(sobj); 
}



      
function sortTable(tableSelector,colIndex, dataType,pstatTr,pendTr){  
    //选择表格  
    var table = $(tableSelector);
    var allRows=table.find('tr');  //找到所有行   
    var oleFirstRow=allRows.slice(0,1).clone(true);//取第一行;
    //计算排序开始行及结束行.
    var startTr= 0;
    var endTr=allRows.length;
    if(pendTr !=null && pendTr <endTr)endTr =pendTr+1;
    if(pstatTr !=null && pstatTr >startTr )startTr =pstatTr;
    if(startTr >endTr )startTr =endTr;
    //    
    //判断上一次排列的列和现在需要排列的是否同一个。
    var order=-1;     
    if (table.data('sortCol')!=null && table.data('sortCol') == colIndex) { 
        order=parseInt(table.data('order'))*-1;
    } 
    //使用数组的sort方法，传进排序函数  
    var sortRows = allRows.slice(startTr,endTr); //提取排序行. 
    sortRows.sort(compareEle(colIndex, dataType,order)); 
    //
    var sortRowsClone=sortRows.clone(true);
    var si=0;
    for(i=startTr;i<endTr;i++){
        $(allRows[i]).replaceWith($(sortRowsClone[si]));
        si++;
    }
    //记录最后一次排序的列索引     
    table.data('sortCol',colIndex); 
    table.data('order',order); 
    //
    //----重新计算列宽开始-----
    table.find('tr').find('td').width('auto');//排序后表所有行单元格置为auto;
    var oleFirstRowTds=oleFirstRow.find('td');
    var newFirstRowTds=table.find('tr').slice(0,1).find('td');
    newFirstRowTds.each(function(){
        var itemtd=$(this);
        itemtd.css('width',$(oleFirstRowTds[newFirstRowTds.index(itemtd)]).css('width'));
    });
    //----重新计算列宽结束-----
 } 
//-------------------------------------------------------------------------------------------    
//排序函数，colIndex表示列索引，dataType表示该列的数据类型     
function compareEle(colIndex, dataType,order) {     
    return function (oTR1, oTR2) { 
             var rs=0; 
             var vValue1 = convertDataType(getCellsValue(oTR1.cells[colIndex]), dataType);     
             var vValue2 = convertDataType(getCellsValue(oTR2.cells[colIndex]), dataType);     
             if (vValue1 < vValue2) {     
                 rs= -1;     
             } else if (vValue1 > vValue2) {     
                 rs= 1;     
             } else {     
                 rs= 0;     
             } 
             return rs*order;    
        };     
 }  
//将列的类型转化成相应的可以排列的数据类型     
function convertDataType(sValue, dataType) {     
     switch(dataType) {     
     case "int":     
         return parseInt(sValue);     
     case "float":     
         return parseFloat(sValue);     
     case "date":     
         return new Date(Date.parse(sValue));     
     default:     
         return sValue.toString();     
     }     
}   
//取单元格值  
function getCellsValue(ob){     
     if (ob.textContent != null)     
     return ob.textContent;     
     var s = ob.innerText;     
     return s.substring(0, s.length);     
}  
