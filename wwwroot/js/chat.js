"use strict";
var connection = new signalR
    .HubConnectionBuilder()
    .withUrl("/chatHub")
    .withAutomaticReconnect()
    .build();

function createMenuOptionsMS(msId, userId, chatroomId) {
    // console.log(msId, userId, chatroomId);
    var ms = {
        Id: msId // Thay đổi Id của tin nhắn theo cách bạn cần
    };

    var currentUser = {
        Id: userId // Thay đổi Id của người dùng hiện tại theo cách bạn cần
    };

    var currentChatroom = {
        Id: chatroomId // Thay đổi Id của phòng chat hiện tại theo cách bạn cần
    };

    var optionMessageIconContainer = document.createElement("div");
    optionMessageIconContainer.className = "option_message_icon_container";

    var moreContainer = document.createElement("div");
    moreContainer.className = "more position-relative";
    moreContainer.id = "more_" + ms.Id;

    var moreButton = document.createElement("button");
    moreButton.className = "more-btn";
    moreButton.id = "more-btn_" + ms.Id;
    moreButton.innerHTML = '<i class="bi bi-three-dots-vertical"></i>';
    moreButton.onclick = function (event) {
        showMenu(event, ms.Id);
    };

    var moreMenu = document.createElement("div");
    moreMenu.className = "more-menu";
    moreMenu.id = "more-menu_" + ms.Id;

    var moreMenuCaret = document.createElement("div");
    moreMenuCaret.className = "more-menu-caret";

    var moreMenuCaretOuter = document.createElement("div");
    moreMenuCaretOuter.className = "more-menu-caret-outer";

    var moreMenuCaretInner = document.createElement("div");
    moreMenuCaretInner.className = "more-menu-caret-inner";

    moreMenuCaret.appendChild(moreMenuCaretOuter);
    moreMenuCaret.appendChild(moreMenuCaretInner);

    var moreMenuItems = document.createElement("ul");
    moreMenuItems.className = "more-menu-items";
    moreMenuItems.setAttribute("tabindex", "-1");
    moreMenuItems.setAttribute("role", "menu");
    moreMenuItems.setAttribute("aria-labelledby", "more-btn");
    moreMenuItems.setAttribute("aria-hidden", "true");

    var moreMenuItem = document.createElement("li");
    moreMenuItem.className = "more-menu-item";

    var unsendButton = document.createElement("button");
    unsendButton.className = "more-menu-btn";
    unsendButton.setAttribute("type", "button");
    unsendButton.setAttribute("role", "menuitem");
    unsendButton.innerHTML = "Unsend";
    unsendButton.onclick = function () {
        handleUnSendMs(currentUser.Id.toString(), parseInt(ms.Id), currentChatroom.Id);
    };

    moreMenuItem.appendChild(unsendButton);
    moreMenuItems.appendChild(moreMenuItem);

    moreMenu.appendChild(moreMenuCaret);
    moreMenu.appendChild(moreMenuItems);

    moreContainer.appendChild(moreButton);
    moreContainer.appendChild(moreMenu);

    optionMessageIconContainer.appendChild(moreContainer);

    return optionMessageIconContainer;
}

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;
// userItem.Id, message, sender.ImageUrl, "right", time.ToString(), chatRoomGroup.Id, msId
connection.on("ReceiveMessage", function (user, message, imageUrl, leftOrRight, time, chatRoomGroupId, type, connectionRoomCall, msId) {
    const url = window.location.href;
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = message;

    if (leftOrRight == "left") {
        if (url.includes(chatRoomGroupId)) {

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
                pTextLeft.id = `message_text_${msId}`
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
            pTextLeft.id = `message_text_${msId}`
            pTextLeft.classList.add("long_text");
            pTextLeft.textContent = encodedMsg;

            // Ghép các phần tử vào nhau
            divMessageItemFriendText.appendChild(divMessageItemImgContainer);
            divMessageItemFriendText.appendChild(pTextLeft);
            divMessageItemFriendText.appendChild(createMenuOptionsMS(msId, user, chatRoomGroupId))

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


function handleAddToastMessageCall(userId, receiverId, connectionRoomCall, chatRoomId) {

    connection.invoke("SendToastMessage", userId, receiverId, connectionRoomCall, chatRoomId).catch(function (err) {
        return console.error(err.toString());
    })
}

function handleSendCallMessage(connectionRoomCall, userName, chatRoomId) {
    var user = document.getElementById("userInput").value;
    var receiverConnectionId = document.getElementById("receiverId").value;
    var message = userName + " has opened a call message!";
    if (message != " ") {
        connection.invoke("SendCallMessageToUser", user, receiverConnectionId, message, connectionRoomCall, chatRoomId).catch(function (err) {
            return console.log(err.toString());
        });
        handleAddToastMessageCall(user, receiverConnectionId, connectionRoomCall, chatRoomId)
    }
    window.location.href = `/call/${connectionRoomCall}`
}

function handleAddToastMessage(userId, message, chatRoomId) {
    connection.invoke("SendToastMessageNof", userId, message, chatRoomId).catch(function (err) {
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
                connection.invoke("SendToUser", user, message, chatRoomId, arrayImageMessages).catch(function (err) {
                    return console.log(err.toString());
                });
                handleAddToastMessage(user, message, chatRoomId)
            }
            event.preventDefault();
        }
    }
}

async function handleUnSendMs(userId, messageId, chatRoomId) {
    console.log("Check >>> ", userId, messageId, chatRoomId);
    try {
        await fetch(`/Chat/UnsendMessage?messageId=${messageId}`)
            .then(response => response.json())
            .then(data => {
                var message_text = document.getElementById(`message_text_${messageId}`)
                if (message_text) {
                    message_text.style.color = "white"
                    message_text.textContent = "Unsended"
                    console.log("Check unsendmessagetouser >>> ", userId, messageId, chatRoomId);
                    connection.invoke("UnSendMessageToUser", userId, messageId, chatRoomId).catch(function (err) {
                        return console.log(err.toString());
                    });

                }
                console.log(data)
            })
    } catch (err) {
        console.log(err.toString());
    }
}

connection.on("ReceiveUnsendMessage", function (userId, messageId, chatRoomId) {
    console.log("check messageId >>> ", messageId);
    var message_text = document.getElementById(`message_text_${messageId}`)
    if (message_text) {
        message_text.style.color = "white"
        message_text.textContent = "Unsended"
    }
})
var arrayImageMessages = [];
connection.on("ImageUploaded", function (imageData) {
    // Handle newly uploaded image
    arrayImageMessages.push(imageData);
    console.log(arrayImageMessages);
    const imgElement = document.createElement("img");
    imgElement.src = imageData;
    imgElement.classList.add('message_image')
    var message_image_input_container = document.getElementById('message_image_input_container')
    if (message_image_input_container) {
        message_image_input_container.appendChild(imgElement);
    }
});



connection.start().then(function () {
    connection.invoke("GetConnectionId").then(function (id) {
        console.log("Connected");
        if (document.getElementById("uploadImage")) {
            // document.getElementById("uploadImage").addEventListener("change", function (event) {
            //     const file = event.target.files[0];
            //     const reader = new FileReader();
            //     reader.onload = function (event) {
            //         const imageData = event.target.result;
            //         if (connection.state === signalR.HubConnectionState.Connected) {
            //             connection.invoke("UploadImage", imageData).catch(err => console.error(err));
            //         } else {
            //             console.error("Connection is not in the 'Connected' state.");
            //             // Handle the situation appropriately, e.g., reconnect or notify the user
            //         }

            //     };
            //     reader.readAsDataURL(file);
            //     console.log('da doc file xong');
            // });
            document.getElementById("uploadImage").addEventListener("change", function (event) {
                var userId = document.getElementById('userInput');
                console.log(userId.value.toString());
                const files = event.target.files;
                for (let i = 0; i < files.length; i++) {
                    const file = files[i];
                    const reader = new FileReader();
                    reader.onload = function (event) {
                        const imageData = event.target.result;
                        if (connection.state === signalR.HubConnectionState.Connected) {
                            connection.invoke("UploadImage", imageData).catch(err => console.error(err));
                        } else {
                            console.error("Connection is not in the 'Connected' state.");
                            // Handle the situation appropriately, e.g., reconnect or notify the user
                        }
                    };
                    reader.readAsDataURL(file);
                }
                console.log('Đã đọc file xong');
            });
        }
    });

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