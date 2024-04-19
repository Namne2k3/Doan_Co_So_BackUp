let userId = null
let localStream = null
function handleChangeIconVideoActions(type) {
    if (type == 'mic') {
        var element = document.getElementById('mic_icon');
        if (element) {
            if (element.classList.contains('bi-mic-fill')) {
                element.classList.remove("bi-mic-fill")
                element.classList.add("bi-mic-mute-fill")
            } else {
                element.classList.remove('bi-mic-mute-fill')
                element.classList.add('bi-mic-fill')
            }
            handleToggleMicro()
        }
    }
    if (type == 'video') {
        var element = document.getElementById('video_icon');
        if (element) {
            if (element.classList.contains('bi-camera-video-fill')) {
                element.classList.remove("bi-camera-video-fill")
                element.classList.add("bi-camera-video-off-fill")
            } else {
                element.classList.remove('bi-camera-video-off-fill')
                element.classList.add('bi-camera-video-fill')
            }
            handleToogleCamera()
        }
    }
    if (type == 'volume') {
        var element = document.getElementById('volume_icon');
        if (element) {
            if (element.classList.contains('bi-volume-up-fill')) {
                element.classList.remove("bi-volume-up-fill")
                element.classList.add("bi-volume-mute-fill")
            } else {
                element.classList.remove('bi-volume-mute-fill')
                element.classList.add('bi-volume-up-fill')
            }
            handleToggleAudio()
        }
    }
}
const Peers = {}

//2. khoi tao ket noi toi signalr
var callconnection = new signalR.HubConnectionBuilder()
    .withUrl("/meeting").build()


// khoi chay ket noi toi signalr hub
const startSignalR = async () => {
    await callconnection.start();
    await callconnection.invoke("JoinRoom", ROOM_ID, userId)

    callconnection.on('user-connected', userId => {
        console.log(`User connected: ${userId}`)
    })
}

//3. khoi tao peer
const myPeer = new Peer();

//4. khoi chay mypeer
// hàm này được chạy mỗi khi peer khởi động và có được một id đại diện duy nhất từ máy chủ peerjs
myPeer.on('open', id => {
    userId = id;
    console.log("Check userId >>> ", userId);
    const startSignalR = async () => {
        await callconnection.start();
        await callconnection.invoke("JoinRoom", ROOM_ID, userId)
    }
    startSignalR();
})

// section stream video
const videoGrid = document.querySelector('[video-grid]')
const myVideo = document.createElement('video')
myVideo.muted = false;

// ( hàm này chỉ được gọi lần đầu tiên )
navigator.mediaDevices.getUserMedia({
    audio: true,
    video: true,
}).then(stream => {
    addVideoStream(myVideo, stream)
    localStream = stream
})

callconnection.on('user-connected', id => {

    // kiểm tra có phải user ban đầu khởi tạo callroom ko
    // nếu ko thì connect tới user mới

    console.log("userId >>> ", userId)
    console.log("Id >>> ", id)
    if (userId === id)
        return;

    console.log(`User connected: ${id}`)
    connectNewUser(id, localStream);
})

callconnection.on("user-disconnected", id => {
    console.log(`User disconnected: ${id}`)

    if (Peers[id])
        Peers[id].close();
})

// khi có 1 user mới truy cập cuộc gọi
// hàm on call này sẽ được kích hoạt
// tác dụng để cho phép những hình ảnh của ta được truyền tới user mới
// từ đó user mới có thể thấy được hình ảnh của ta
myPeer.on('call', call => {
    call.answer(localStream)
    console.log("thuc hien lenh call")

    const userVideo = document.createElement('video')

    // them hình ành video của mình truyền sang user mới
    call.on('stream', userVideoStream => {
        addVideoStream(userVideo, userVideoStream)
    })

    call.on('close', () => {
        userVideo.remove()
    })
    Peers[userId] = call
})

const handleToggleMicro = () => {
    const tracks = localStream.getAudioTracks();

    // Đảm bảo rằng có track audio tồn tại
    if (tracks.length > 0) {
        // Lặp qua từng track và chuyển đổi trạng thái enabled
        tracks.forEach(track => {
            // Đảm bảo rằng track có hỗ trợ set enabled
            if ('enabled' in track) {
                track.enabled = !track.enabled; // Đảo ngược trạng thái của track
            }
        });
    }
}

const handleToogleCamera = () => {
    const tracks = localStream.getVideoTracks();

    // Đảm bảo rằng có track video tồn tại
    if (tracks.length > 0) {
        // Lặp qua từng track và chuyển đổi trạng thái enabled
        tracks.forEach(track => {
            // Đảm bảo rằng track có hỗ trợ set enabled
            if ('enabled' in track) {
                track.enabled = !track.enabled; // Đảo ngược trạng thái của track
            }
        });
    }
}
const addVideoStream = (video, stream) => {
    video.srcObject = stream;
    video.addEventListener('loadedmetadata', () => {
        video.muted = false;
        video.audio = false;
        video.play()
    })

    videoGrid.appendChild(video)
}

const connectNewUser = (userId, localStream) => {
    const userVideo = document.createElement('video')
    const call = myPeer.call(userId, localStream)

    console.log('connect new user')

    // thêm hình ảnh video của user mới bên phía mình
    call.on('stream', userVideoStream => {

        console.log('thuc hien connectusermoi')
        addVideoStream(userVideo, userVideoStream)
    })

    call.on('close', () => {
        userVideo.remove()
    })
    Peers[userId] = call
}