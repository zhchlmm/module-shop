App({
  onLaunch: function () {
    try {
      let userInfo = wx.getStorageSync('userInfo');
      if (userInfo) {
        this.globalData.userInfo = JSON.parse(userInfo);
      }
      this.globalData.token = wx.getStorageSync('token');

      // wx.getSystemInfo({
      //   success: e => {
      //     this.globalData.StatusBar = e.statusBarHeight;
      //     let custom = wx.getMenuButtonBoundingClientRect();
      //     this.globalData.Custom = custom;
      //     this.globalData.CustomBar = custom.bottom + custom.top - e.statusBarHeight;
      //   }
      // })

      // 登录
      // wx.login({
      //   success: res => {
      //     console.log(res);
      //     // 发送 res.code 到后台换取 openId, sessionKey, unionId
      //   }
      // })

      /*
      // 获取用户信息
      wx.getSetting({
        success: res => {
          if (res.authSetting['scope.userInfo']) {
            // 已经授权，可以直接调用 getUserInfo 获取头像昵称，不会弹框
            wx.getUserInfo({
              success: res => {
                // 可以将 res 发送给后台解码出 unionId
                this.globalData.userInfo = res.userInfo

                // 由于 getUserInfo 是网络请求，可能会在 Page.onLoad 之后才返回
                // 所以此处加入 callback 以防止这种情况
                if (this.userInfoReadyCallback) {
                  this.userInfoReadyCallback(res)
                }
              }
            })
          }
        }
      })
      */

    } catch (e) {
      console.log(e);
    }
  },
  globalData: {
    title: '菜蕊商城',
    desc: '菜蕊商城提供新鲜蔬菜、水果、食用农产品、鲜蛋、鲜肉、水产品等批发和零售，和生鲜食材全供应链服务。',
    defaultAvatar: 'http://yanxuan.nosdn.127.net/8945ae63d940cc42406c3f67019c5cb6.png',
    userInfo: {
      name: '',
      avatar: 'http://yanxuan.nosdn.127.net/8945ae63d940cc42406c3f67019c5cb6.png'
    },
    token: '',
  }
})