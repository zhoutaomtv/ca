//显示层
function doShowTable(obj) {

    //原始高
    var yHeight = document.body.scrollHeight;

    document.all[obj].style.display = "block";
    var x = event.clientX, y = event.clientY;
    var yTop = document.body.scrollTop;

    if (event.clientX > 500) {
        x -= 510;
    }
    else {
        //x += 10;
    }
    y = event.clientY + yTop - 110;

    /* if(event.clientY < 150)
    {
    y -= 120;
    }
    else
    {
    y += 10;
    }*/


    document.all[obj].style.left = x;
    document.all[obj].style.top = y;

}
//隐藏层
function doHideLetter(obj) {
    document.all[obj].style.display = "none";
}