// 改变时间格式
function changeDateFormat(cellval) {
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
    var min = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
    return date.getFullYear() + "-" + month + "-" + currentDate+" "+hour+":"+min;
}
function LoadData(pageIndex) {
    //var href = window.location.href;
    var url = "/Mood/List/?pageIndex=" + pageIndex;
    //alert(href);
    //if (href.lastIndexOf("?")==-1) {
    //    url="/Home/List/?pageIndex=" + pageIndex;
    //}
    //else {
    //    url="/Home/List/"+pageIndex+"?CategoryId="+href.substring(href.lastIndexOf("=")+1)
    //}
    //alert(url);
    $.post(url, null, function (data) {
        if (data.Status == 0 && data.Msg == "success") {
            var itemsHtml = '';

            if (data.Data.Items == null) {
                itemsHtml = '<h4>暂无数据！  -_- </h4>';
                $("#tabContent").html(itemsHtml);
                return;
            }
            //alert((data.Data.Items).length);
            //这里的 for 循环应该用 (data.Data.Items).length ，用查询到数据的结果数
            for (var i = 0; i < (data.Data.Items).length; i++) {
                //itemsHtml += "<li class='item'><a href='/Home/Details/" + data.Data.Items[i].pId + "' class='btn btn-primary btn-lg'>" + data.Data.Items[i].Name + "</a></li>";
                //alert(data.Data.Items[i].MoodContent);
                itemsHtml += "<tr><td align='left' colspan='4'>" + data.Data.Items[i].MoodContent + "</td></tr><tr><td colspan='3'></td><td align='right'><a href='#' onclick='DelMood(" + data.Data.Items[i].MoodId + ")'>删除</a> &nbsp;&nbsp; " + changeDateFormat(data.Data.Items[i].MoodTime); + " </td></tr>";
            }
            //alert(itemsHtml);
            //接受数据
            $("#tabContent").html(itemsHtml);
            //var tab = document.getElementById("tabContent");
            //tab.innerHTML = itemsHtml;
            makePageBar(LoadData, document.getElementById("divPageBar"), data.Data.PageIndex, data.Data.PageCount, 3, data.Data.RowsCount);
        }
        else {
            alert('发生异常' + data.Msg);
        }
    });
}
function AddMood() {
    $.post("/Mood/Add/", { "MoodContent": $("#MoodContent").text() }, function (data) {
        if (data.Status == 0 && data.Msg == "success") {
            $("#MoodContent").text('');
            //alert('Add Success');
            LoadData(1);
        }
        else {
            alert('发生异常' + data.Msg);
        }
    });
    //取消表单提交
    return false;
}

function DelMood(id) {
    if (confirm('确定删除？')) {
        $.post("/Mood/Delete/" + id, null, function (data) {
            if (data.Status == 0 && data.Msg == "success") {
                //alert('Delete Success');
                LoadData(1);
            }
            else {
                alert('发生异常' + data.Msg);
            }
        });
    }
}