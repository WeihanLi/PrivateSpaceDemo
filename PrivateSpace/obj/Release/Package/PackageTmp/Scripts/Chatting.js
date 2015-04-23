function SendMsg() {
    var input = document.getElementById("lite_cmd_inputbox");
    var msg = encodeURI(input.value);
    input.value = "";
    if (msg == "" || msg == null) {
        return;
    }
    else {
        DomOperSend(decodeURI(msg));
        GetReply(msg);
    }
}
//获取回复
function GetReply(msg) {
    //Ajax 请求回复
    //alert(msg);
    $.get("../Chat/GetReply/?msg=" + msg, null, function (data, status) {
        //alert(data.Status);
        if (data.Msg == "success"&&data.Status==0) {
            DomOperReceive(data.Data);
        }
        else {
            alert(data.Msg);
        }
    },"json");
    //修改请求方案
    //私密空间.API.ChattingService.GetResponse(msg, DomOperReceive);
}
//显示发送的消息
function DomOperSend(msg) {
    var content = '<div class="lite_conversation_item"> <div class="lite_conv_innerwrap_right"><div class="lite_conv_right">' + msg + '</div></div></div>';
    var container = document.getElementById("lite_content");
    container.innerHTML = container.innerHTML + content;
    container.scrollTop = container.scrollHeight;
}
//显示请求到的回复
function DomOperReceive(msg) {
    var content = '<div class="lite_conversation_item"> <div class="lite_conv_innerwrap_left"><div class="lite_conv_left">' + msg + '</div></div></div>';
    var container = document.getElementById("lite_content");
    container.innerHTML = container.innerHTML + content;
    container.scrollTop=container.scrollHeight;
}
function SendChatting(){
    //if (window.event.keyCode == 13) {//如果取到的键值是回车
    SendMsg();
    return false;
}