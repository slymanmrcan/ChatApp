$(document).ready(()=>{
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub",{withCredentials:false})
        .withAutomaticReconnect([1000,1000,1000,1000,2000,3000,4000,5000,]) //burada ilk bağlantı var sonrasında koptuğunda kullanlı
        .build();
    async function start(){
        try {
            await connection.start();
        } catch (error) {
            setTimeout(() => {
                start();
            }, 2000);
        }
    }
    const status =$("#userStatus");
    const connectionStatus =$("#connectionStatus");
    connection.start();
    let nickName = "";
    $("#sendNickNamebtn").click(()=>{
        nickName =$("#nickName").val();
        connection.invoke("GetNickName",nickName)
            .then()
            .catch(error => console.log(error));
    })
    connection.on("clientJoined",nickName=>{
        $("#userStatus").html(`${nickName} giriş ypatı`);
        animation();
    })
    connection.on("ReceiveMessage",message=>{
        $("#messages").append(message,"<br>");
    });
    connection.on("ReceiveMessageClient",(message,clientId)=>{
        $("#messages").append(message,"<br>",clientId);
    });
    $("body").on("click", "#clients li", function() {  // li elemanlarına click için
        $("#clients li").removeClass("active"); // tüm li'lerden active'i kaldır
        $(this).addClass("active");
        let selectedNickname = $(this).find(".name").text();
        $("h6[header-nickname]").text(selectedNickname);
    });
    connection.on("clients", (client) => {
        const filteredClient = client.filter(client => client.nickName !== nickName);
        $("#clients").empty();
        let clientList = $("<ul class=\"list-unstyled chat-list mt-2 mb-0\"></ul>");
        filteredClient.forEach(client => {
            clientList.append(`<li class="clearfix">
                    <img src="https://bootdey.com/img/Content/avatar/avatar1.png" alt="avatar">
                        <div class="about">
                            <div class="name">${client.nickName} {çevrimiçi}</div>
                            <div class="status"> <i class="fa fa-circle offline"></i>son mesaj burada olsun </div>
                        </div>
                 </li>`);
        });
        $("#clients").append(clientList);
    });
    connection.onreconnected(connectionId=>{
        connectionStatus.html("bağlantı kuruldu")
    });
    connection.onclose(connectionId=>{
        connectionStatus.html("bağlantı kurulamadı ");
    });
    connection.onreconnecting(error=>{
        connectionStatus.html("bağlantı kuruluyor....");
    });
    let _connectionId = "";
    connection.on("ReceiveConnectionId",connectionId=>{
        _connectionId = connectionId;
        $("#connectionId").html(`Connection Id :${connectionId}`);
    });
    
    /*
    $("#sendmessagebtn").click(()=>{
        let message = $("#messagetext").val();
        let connectionIds = $("#connectionIds").val().split(",");
        // connection.invoke("SendMessageAsync",message).catch(error=>console.log(`mesaj gödnderiliken hata oldu ${error}`));
         connection.invoke("SendMessageClient",message,connectionIds).catch(error=>console.log(`mesaj gödnderiliken hata oldu ${error}`));
    });
    */
    $("#sendmessagebtn").click(()=>{
        let message = $("#messagetext").val();
        connection.invoke("SendMessageAllClient",message).catch(error=>console.log(`mesaj gödnderiliken hata oldu ${error}`));
        // connection.invoke("SendMessageClient",message,connectionIds).catch(error=>console.log(`mesaj gödnderiliken hata oldu ${error}`));
    });

    $("#messagesendbtn").click(() => {
        const clientName = $("#clients li.active .name").first().html();
        const clientName1 = clientName.replace(" {çevrimiçi}","")
        let message = $("#messagetextarea").val();

        connection.invoke("SendMessageClientAsync", message, clientName)
            .then(() => {
                // Mesajı gönderdikten sonra textarea'yı temizle
                $("#messagetextarea").val("");

                // Mesajı ekle
                $(".message-data.text-right").after(
                    $("<div>").addClass('message-wrapper float-right').append(
                        $("<div>").addClass('nickname').text(clientName1), // Göndericinin adı
                        $("<div>").addClass('message other-message').text(message), // Mesaj içeriği
                        $("<br>") // Alt satıra geçiş
                    )
                );
            })
            .catch((error) => {
                console.error("Message send failed:", error);
            });
    });
    connection.on("receiveclientmessage", (message, senderClient) => {
        $(".messages").append(
            $("<div>").text(senderClient) // Birinci div oluşturuluyor.
        ).append(
            $("<div>").addClass('message my-message').text(message) // İkinci div oluşturuluyor ve sınıf ekleniyor.
        ).append("<br>"); // Line break ekleniyor.
    });
    connection.on("disconnectClientId", (disconnectedId) => {
        console.log("Disconnected user:", disconnectedId);

        // Örneğin UI'dan bu kullanıcıyı kaldırabilirsin
        $(`#clients li[data-connectionid="${disconnectedId}"]`).remove();

        // Veya offline olarak işaretleyebilirsin
        $(`#clients li[data-connectionid="${disconnectedId}"] .status`)
            .html('<i class="fa fa-circle offline"></i> offline');
    });
    function animation(){
        status.fadeIn(2000,()=>{setTimeout(() => {
            status.fadeOut(2000)
        }, 2000);})
    }
});