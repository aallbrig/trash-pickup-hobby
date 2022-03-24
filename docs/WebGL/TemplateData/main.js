var canvas = document.querySelector("#unity-canvas");

var config = {
    dataUrl: "Build/WebGL.data",
    frameworkUrl: "Build/WebGL.framework.js",
    codeUrl: "Build/WebGL.wasm",
    streamingAssetsUrl: "StreamingAssets",
    companyName: "DefaultCompany",
    productName: "trash-pickup-video-game",
    productVersion: "0.0.25",
    devicePixelRatio: 1,
}

createUnityInstance(canvas, config);
