var left={};
/*
显示菜单
菜单打开
导航

*/

left.loadMenu = function() {
    var _menu_nodes = [];
    var _znodes;
    var appKey = request.queryString("appKey");
    if (appKey != undefined) {
        _znodes = parent.CateRest.UserMenu(appKey);
    }
    else {
        _znodes = parent.CateRest.UserMenu();
    }
    //得到用户菜单
    _menu_nodes = _znodes;
    var hasChild = function(id) {
        var _childs = [];
        $(_menu_nodes).each(function(i, item) {
            if (id == item.parentId) {
                _childs.push(item);
            }
        });
        return _childs;
    }
    var addItem = function(item) {
        var temp = '<li><a id="1_' + item.id + '"  href="javascript:;" title="' + item.name + '" onclick="parent.main.forleftOpenMenu(\'' + item.id + '\',\'' + item.name + '\',\'' + item.pageUrl + '\');">' + item.name + '</a></li>';
        return temp;
    }
    if (_menu_nodes && _menu_nodes.length > 0) {
        var _shtml = "";
        var n = 1;
        $(_menu_nodes).each(function(i, item) {
            if (item.parentId == "" || item.parentId == "0") {

            //     _shtml += '<ul class="divMenu"><li><a href="javascript:;" onclick="menuControl(\'' + item.id + '\')"><img style="border:none;" src="Theme/Menu/menu' + n + '.png" /><span style="margin-left:10px;">'
            // + item.name + '<span></a></li></ul><div id="' + item.id + '" class="content" flag="firstDiv" ';

                  _shtml += ' <div  class="content_menu_list" id="dv_menu"><div class="list_title"><a href="#" onclick="menuControl(\'' + item.id + '\')"> <img  src="./../Bdc/img/'+n+'.png" /><p>'
            + item.name + '</p><img src="./../Bdc/img/jty.png"></a></div><ul   id="' + item.id + '" class="list ';



                if (item.name != "业务处理" && item.name != "业务办理查询" && item.name != "数据汇交" && request.queryString("appKey") != "11acfdf4-3bcb-4ffc-86ae-9b706d3d0654") {
                    // 当只有一个一级菜单，则默认展开第一个菜单
                    _shtml += ' isOpen';
                }

                _shtml += '">';
                var _childs = hasChild(item.id);
                if (_childs && _childs.length > 0) {
                    //添加菜单
                    // _shtml += "<ul class='MM'>";
                    $(_childs).each(function(i, child_item) {
                        _shtml += addItem(child_item);
                    }
                   );
                    _shtml += "</ul>";
                }
                _shtml += "</div>";
                n++;
            }
        });
        $("#dv_menu").html(_shtml);
        //$("#dv_menu").render();
        //展开第一项
        //$(".titlebar").click();
    }
}
//加载
$(function(){
   left.loadMenu();

});











































































