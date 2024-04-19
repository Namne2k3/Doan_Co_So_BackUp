"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;
connection.on("ReceiveMessage", function (user, message, imageUrl, leftOrRight, time, type, connectionRoomCall) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = message;

    if (leftOrRight == "left") {
        if (type == "call") {
            var divMessageItem = document.createElement("div");
            divMessageItem.classList.add("message_item");

            var divMessageItemFriendText = document.createElement("div");
            divMessageItemFriendText.classList.add("message_item_friend_text");

            var divMessageItemImgContainer = document.createElement("div");
            divMessageItemImgContainer.classList.add("message_item_img_container");
            var img = document.createElement("img");
            img.src = imageUrl; // Thay đổi đường dẫn ảnh tùy theo imageUrl
            img.alt = "user_image";
            divMessageItemImgContainer.appendChild(img);

            var pTextLeft = document.createElement("p");
            pTextLeft.classList.add("long_text");
            pTextLeft.textContent = encodedMsg;

            // Ghép các phần tử vào nhau
            divMessageItemFriendText.appendChild(divMessageItemImgContainer);

            // Tạo một thẻ a
            var anchorElement = document.createElement('a');

            // Thêm các thuộc tính và giá trị cho thẻ a

            anchorElement.href = `/call/${connectionRoomCall}`
            anchorElement.classList.add('btn', 'btn-outline-light', 'call_icon');

            // Tạo một thẻ i
            var iconElement = document.createElement('i');
            iconElement.classList.add('bi', 'bi-telephone-outbound');

            // Thêm thẻ i vào thẻ a
            anchorElement.appendChild(iconElement);

            pTextLeft.appendChild(anchorElement)

            divMessageItemFriendText.appendChild(pTextLeft);

            var p_time = document.createElement("p");
            p_time.textContent = time
            p_time.style.textAlign = "left"

            divMessageItem.appendChild(divMessageItemFriendText);
            divMessageItem.appendChild(p_time);

            // Thêm div vào danh sách tin nhắn
            document.getElementById("messages_display_container").appendChild(divMessageItem);
            var messages_display_container = document.getElementById("messages_display_container")
            if (messages_display_container) {
                messages_display_container.scrollTop = messages_display_container.scrollHeight;
            }
        } else {
            var divMessageItem = document.createElement("div");
            divMessageItem.classList.add("message_item");

            var divMessageItemFriendText = document.createElement("div");
            divMessageItemFriendText.classList.add("message_item_friend_text");

            var divMessageItemImgContainer = document.createElement("div");
            divMessageItemImgContainer.classList.add("message_item_img_container");
            var img = document.createElement("img");
            img.src = imageUrl; // Thay đổi đường dẫn ảnh tùy theo imageUrl
            img.alt = "user_image";
            divMessageItemImgContainer.appendChild(img);

            var pTextLeft = document.createElement("p");
            pTextLeft.classList.add("long_text");
            pTextLeft.textContent = encodedMsg;

            // Ghép các phần tử vào nhau
            divMessageItemFriendText.appendChild(divMessageItemImgContainer);
            divMessageItemFriendText.appendChild(pTextLeft);

            var p_time = document.createElement("p");
            p_time.textContent = time
            p_time.style.textAlign = "left"

            divMessageItem.appendChild(divMessageItemFriendText);
            divMessageItem.appendChild(p_time);

            // Thêm div vào danh sách tin nhắn
            document.getElementById("messages_display_container").appendChild(divMessageItem);
            var messages_display_container = document.getElementById("messages_display_container")
            if (messages_display_container) {
                messages_display_container.scrollTop = messages_display_container.scrollHeight;
            }
        }
        // Tạo các phần tử HTML
    } else {
        if (type == "call") {
            var divMessageItem = document.createElement("div");
            divMessageItem.classList.add("message_item");

            var divMessageItemFriendText = document.createElement("div");
            divMessageItemFriendText.classList.add("message_item_user_text");

            var divMessageItemImgContainer = document.createElement("div");
            divMessageItemImgContainer.classList.add("message_item_img_container");
            var img = document.createElement("img");
            img.src = imageUrl; // Thay đổi đường dẫn ảnh tùy theo imageUrl
            img.alt = "user_image";
            divMessageItemImgContainer.appendChild(img);

            var pTextLeft = document.createElement("p");
            pTextLeft.classList.add("long_text");
            pTextLeft.textContent = encodedMsg;

            // Ghép các phần tử vào nhau
            divMessageItemFriendText.appendChild(divMessageItemImgContainer);

            // Tạo một thẻ a
            var anchorElement = document.createElement('a');

            // Thêm các thuộc tính và giá trị cho thẻ a

            anchorElement.href = `/call/${connectionRoomCall}`
            anchorElement.classList.add('btn', 'btn-outline-light', 'call_icon');

            // Tạo một thẻ i
            var iconElement = document.createElement('i');
            iconElement.classList.add('bi', 'bi-telephone-outbound');

            // Thêm thẻ i vào thẻ a
            anchorElement.appendChild(iconElement);

            pTextLeft.appendChild(anchorElement)
            divMessageItemFriendText.appendChild(pTextLeft);

            document.getElementById("messageInput").value = ""

            var p_time = document.createElement("p");
            p_time.textContent = time
            p_time.style.textAlign = "right"

            divMessageItem.appendChild(divMessageItemFriendText);
            divMessageItem.appendChild(p_time);

            // Thêm div vào danh sách tin nhắn
            document.getElementById("messages_display_container").appendChild(divMessageItem);

            var messages_display_container = document.getElementById("messages_display_container")
            if (messages_display_container) {
                messages_display_container.scrollTop = messages_display_container.scrollHeight;
            }
        } else {
            var divMessageItem = document.createElement("div");
            divMessageItem.classList.add("message_item");

            var divMessageItemFriendText = document.createElement("div");
            divMessageItemFriendText.classList.add("message_item_user_text");

            var divMessageItemImgContainer = document.createElement("div");
            divMessageItemImgContainer.classList.add("message_item_img_container");
            var img = document.createElement("img");
            img.src = imageUrl; // Thay đổi đường dẫn ảnh tùy theo imageUrl
            img.alt = "user_image";
            divMessageItemImgContainer.appendChild(img);

            var pTextLeft = document.createElement("p");
            pTextLeft.classList.add("long_text");
            pTextLeft.textContent = encodedMsg;

            // Ghép các phần tử vào nhau
            divMessageItemFriendText.appendChild(divMessageItemImgContainer);
            divMessageItemFriendText.appendChild(pTextLeft);

            document.getElementById("messageInput").value = ""

            var p_time = document.createElement("p");
            p_time.textContent = time
            p_time.style.textAlign = "right"

            divMessageItem.appendChild(divMessageItemFriendText);
            divMessageItem.appendChild(p_time);

            // Thêm div vào danh sách tin nhắn
            document.getElementById("messages_display_container").appendChild(divMessageItem);

            var messages_display_container = document.getElementById("messages_display_container")
            if (messages_display_container) {
                messages_display_container.scrollTop = messages_display_container.scrollHeight;
            }
        }
    }
});
connection.on('ReceiveToastMessageNof', function (userName, message, userImage, chatRoomId) {
    const currentPath = window.location.pathname;
    console.log("check currentPath >>> ", currentPath);
    if (!currentPath.includes('/Chat')) {
        console.log("da chay vao ReceiveToastMessageNof")
        const toastDiv = document.createElement('div');
        toastDiv.classList.add('toast', 'show');
        toastDiv.setAttribute('role', 'alert');
        toastDiv.setAttribute('aria-live', 'assertive');
        toastDiv.setAttribute('aria-atomic', 'true');

        // Tạo phần tử div chứa header
        const headerDiv = document.createElement('div');
        headerDiv.classList.add('toast-header');

        // Tạo ảnh trong header (nếu cần)
        const img = document.createElement('img');
        img.classList.add('rounded', 'me-2', 'mw-100');
        img.setAttribute('src', userImage);
        img.setAttribute('alt', 'user image');
        headerDiv.appendChild(img);

        // Tạo phần tử strong chứa nội dung tiêu đề
        const strong = document.createElement('strong');
        strong.classList.add('me-auto');
        strong.textContent = `${userName} has sent a message: "${message}"`;
        headerDiv.appendChild(strong);

        // Tạo nút close
        const closeButton = document.createElement('button');
        closeButton.setAttribute('type', 'button');
        closeButton.classList.add('btn-close');
        closeButton.setAttribute('data-bs-dismiss', 'toast');
        closeButton.setAttribute('aria-label', 'Close');
        headerDiv.appendChild(closeButton);

        // Thêm header vào toast
        toastDiv.appendChild(headerDiv);

        // Tạo phần tử div chứa body
        const bodyDiv = document.createElement('div');
        bodyDiv.classList.add('toast-body');

        // Tạo nút "Join Call"
        const joinCallButton = document.createElement('a');
        joinCallButton.classList.add('btn', 'btn-outline-dark');
        joinCallButton.href = `/Chat/Details/${chatRoomId}`
        joinCallButton.textContent = 'View Message';
        bodyDiv.appendChild(joinCallButton);

        // Thêm body vào toast
        toastDiv.appendChild(bodyDiv);

        // Thêm toast vào DOM
        document.getElementById("toast_container").appendChild(toastDiv)

        const toastTimeout = 5000; // 5 giây
        setTimeout(() => {
            toastDiv.remove()
        }, toastTimeout);
    }
})
connection.on('ReceiveToastMessage', function (userName, connectionRoomCall, userImage) {

    console.log("da chay vao ReceiveToastMessage")
    // Tạo phần tử div chứa toast
    const toastDiv = document.createElement('div');
    toastDiv.classList.add('toast', 'show');
    toastDiv.setAttribute('role', 'alert');
    toastDiv.setAttribute('aria-live', 'assertive');
    toastDiv.setAttribute('aria-atomic', 'true');

    // Tạo phần tử div chứa header
    const headerDiv = document.createElement('div');
    headerDiv.classList.add('toast-header');

    // Tạo ảnh trong header (nếu cần)
    const img = document.createElement('img');
    img.classList.add('rounded', 'me-2', 'mw-100');
    img.setAttribute('src', userImage);
    img.setAttribute('alt', 'user image');
    headerDiv.appendChild(img);

    // Tạo phần tử strong chứa nội dung tiêu đề
    const strong = document.createElement('strong');
    strong.classList.add('me-auto');
    strong.textContent = `${userName} has open a call message`;
    headerDiv.appendChild(strong);

    // Tạo nút close
    const closeButton = document.createElement('button');
    closeButton.setAttribute('type', 'button');
    closeButton.classList.add('btn-close');
    closeButton.setAttribute('data-bs-dismiss', 'toast');
    closeButton.setAttribute('aria-label', 'Close');
    headerDiv.appendChild(closeButton);

    // Thêm header vào toast
    toastDiv.appendChild(headerDiv);

    // Tạo phần tử div chứa body
    const bodyDiv = document.createElement('div');
    bodyDiv.classList.add('toast-body');

    // Tạo nút "Join Call"
    const joinCallButton = document.createElement('a');
    joinCallButton.classList.add('btn', 'btn-outline-dark');
    joinCallButton.href = `/call/${connectionRoomCall}`
    joinCallButton.textContent = 'Join Call';
    bodyDiv.appendChild(joinCallButton);

    // Thêm body vào toast
    toastDiv.appendChild(bodyDiv);

    // Thêm toast vào DOM
    document.getElementById("toast_container").appendChild(toastDiv)

    const toastTimeout = 5000; // 5 giây
    setTimeout(() => {
        toastDiv.remove()
    }, toastTimeout);
})
function handleAddToastMessageCall(userId, receiverId, connectionRoomCall) {

    connection.invoke("SendToastMessage", userId, receiverId, connectionRoomCall).catch(function (err) {
        return console.error(err.toString());
    })
}

function handleSendCallMessage(connectionRoomCall, userName) {
    var user = document.getElementById("userInput").value;
    var receiverConnectionId = document.getElementById("receiverId").value;
    var message = userName + " has opened a call message!";
    if (message != " ") {
        connection.invoke("SendCallMessageToUser", user, receiverConnectionId, message, connectionRoomCall).catch(function (err) {
            return console.error(err.toString());
        });
        handleAddToastMessageCall(user, receiverConnectionId, connectionRoomCall)
    }
    window.location.href = `/call/${connectionRoomCall}`
}

function handleAddToastMessage(userId, receiverId, message, chatRoomId) {
    connection.invoke("SendToastMessageNof", userId, receiverId, message, chatRoomId).catch(function (err) {
        return console.error(err.toString());
    })
}

function handleSendMessage(event) {
    if (event.keyCode == 13) {
        if (!event.shiftKey) {
            var user = document.getElementById("userInput").value;
            var receiverConnectionId = document.getElementById("receiverId").value;
            var message = document.getElementById("messageInput").value;
            var chatRoomId = document.getElementById('chatRoom_Id').value;
            if (message != " ") {
                connection.invoke("SendToUser", user, receiverConnectionId, message).catch(function (err) {
                    return console.error(err.toString());
                });
                handleAddToastMessage(user, receiverConnectionId, message, chatRoomId)
            }
            event.preventDefault();
        }
    }
}

connection.start().then(function () {
    console.log("Connected");
    connection.invoke("GetConnectionId").then(function (id) {
        //document.getElementById("connectionId").innerText = id;
    });
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

if (document.getElementById("sendButton") != null) {
    document.getElementById("sendButton").addEventListener("click", function (event) {
        var user = document.getElementById("userInput").value;
        var message = document.getElementById("messageInput").value;
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}
if (document.getElementById("sendToUser") != null) {
    document.getElementById("sendToUser").addEventListener("click", function (event) {
        var user = document.getElementById("userInput").value;
        var receiverConnectionId = document.getElementById("receiverId").value;
        var message = document.getElementById("messageInput").value;
        connection.invoke("SendToUser", user, receiverConnectionId, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}