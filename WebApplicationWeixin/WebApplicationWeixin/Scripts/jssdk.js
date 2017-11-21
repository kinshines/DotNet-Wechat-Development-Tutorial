$('#checkJsApi').onclick = function () {
    wx.checkJsApi({
        jsApiList: [
          'getNetworkType',
          'previewImage'
        ],
        success: function (res) {
            alert(JSON.stringify(res));
        }
    });
};