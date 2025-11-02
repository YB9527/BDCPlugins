$(function(){
    // 左侧菜单鼠标移入和点击效果
    var isOpen = false;
    $(".list_title > a")
    .hover(function(){
        $(this).css('color','#007fe2');
        $(this).find('img:eq(1)').attr('src','./../Bdc/img/djjt01.png');
    },function(){
        $(this).css('color','#282828');
        $(this).find('img:eq(1)').attr('src','./../Bdc/img/jty.png');
    })
    .click(function(){
        var $ul = $(this).parent().siblings('ul');
        if(!isOpen){ // 去关闭
             $ul.removeClass('isOpen');
             $(this).find('img:eq(1)').attr('src','./../Bdc/img/jts.png');
        }else{
            $ul.addClass('isOpen');
            $(this).find('img:eq(1)').attr('src','./../Bdc/img/djjt01.png');
        }
        isOpen = !isOpen;
    })
    //动态添加左侧菜单



})