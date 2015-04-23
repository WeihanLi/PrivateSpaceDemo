function changeDateFormat(cellval) {
    var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    return date.getFullYear() + "-" + month + "-" + currentDate;
}
function LoadData(pageIndex) {
    var url = "/Diary/List/?pageIndex=" + pageIndex;
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
                itemsHtml += "<tr><td align='left' colspan='2'><a href='/Diary/Detail/" + data.Data.Items[i].DiaryId + "'>" + data.Data.Items[i].DiaryTitle + "</a></td><td align='right'><a href='/Diary/Edit/" + data.Data.Items[i].DiaryId + "'>编辑</a> <a href='#' onclick='DelDiary(" + data.Data.Items[i].DiaryId + ")'>删除</a> &nbsp;&nbsp;&nbsp;" + changeDateFormat(data.Data.Items[i].DiaryTime); + " </td></tr>";
            }
            //alert(itemsHtml);
            //接受数据
            $("#tabContent").html(itemsHtml);
            makePageBar(LoadData, document.getElementById("divPageBar"), data.Data.PageIndex, data.Data.PageCount, 3, data.Data.RowsCount);
        }
        else {
            alert('发生异常' + data.Msg);
        }
    });
}
function AddDiary(){
    $.post("/Diary/AddDiary/", { diaryTitle: $("#diaryTitle").val(), diaryContent: $("#diaryContent").val() }, function (data) {
            if (data.Status == 0 && data.Msg == "success") {
                //alert('Add Success');
                window.location.href = '/Diary/Index';
            }
            else {
                alert('发生异常' + data.Msg);
            }
    });
    location.href = "/Diary/";
}

function DelDiary(id) {
    if (confirm('Are you sure to delete？')) {
        $.post("/Diary/Delete/" + id, null, function (data) {
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
