const util = require('../../../utils/util.js');
const api = require('../../../config/api.js');
const user = require('../../../services/user.js');
const app = getApp();

Page({
  data: {
    userInfo: app.globalData.userInfo,
    shopName: app.globalData.title,
    showLoginDialog: false
  },
  onLoad: function (options) {
    // 页面初始化 options为页面跳转所带来的参数
  },
  onReady: function () {

  },
  onShow: function () {
    let that = this;
    user.checkLogin().then(res => {
      if (res) {
        that.setData({
          userInfo: app.globalData.userInfo,
        });
      }
    });
  },
  onHide: function () {
    // 页面隐藏

  },
  onUnload: function () {
    // 页面关闭
  },

  onUserInfoClick: function () {
    if (!wx.getStorageSync('token')) {
      // 优化登录逻辑，默认无弹窗登录并获取用户信息，未授权用户信息和手机号时，弹窗进行授权！

      this.onWechatLogin(this);
    }
  },

  showLoginDialog() {
    this.setData({
      showLoginDialog: true
    })
  },

  onCloseLoginDialog() {
    this.setData({
      showLoginDialog: false
    })
  },

  onDialogBody() {
    // 阻止冒泡
  },

  onWechatLogin(that) {

    wx.showLoading({
      title: '正在登录...',
    });

    util.login().then((res) => {
      return util.request(api.LoginByWeixin, {
        code: res,
        nickName: "微信用户", //e.detail.userInfo.nickName,
        avatarUrl: app.globalData.defaultAvatar //e.detail.userInfo.avatarUrl
      }, 'POST');
    }).then((res) => {
      wx.hideLoading();
      // console.log(res)
      if (res.success !== true) {
        wx.showToast({
          title: '微信登录失败',
        })
        return false;
      }

      let userInfo = {
        name: res.data.name,
        avatar: res.data.avatar,
        phoneNumber: res.data.phone,
        roles: res.data.roles
      };

      app.globalData.userInfo = userInfo;
      app.globalData.token = res.data.token;

      wx.setStorageSync('userInfo', JSON.stringify(userInfo));
      wx.setStorageSync('token', res.data.token);

      // 设置用户信息
      that.setData({
        userInfo: userInfo,
        showLoginDialog: !res.data.phone
      });

    }).catch((err) => {
      wx.hideLoading();
      console.log(err);
    })
  },

  onOrderInfoClick: function (event) {
    wx.navigateTo({
      url: '/pages/ucenter/order/order',
    })
  },

  onSectionItemClick: function (event) {

  },

  bindCallMe: function () {
    wx.makePhoneCall({
      phoneNumber: '13523536961'
    });
  },
  getPhoneNumber: function (e) {
    //code	String	动态令牌。可通过动态令牌换取用户手机号。使用方法详情 phonenumber.getPhoneNumber 接口
    console.log(e.detail);

    const mobileCode = e.detail.code;
    let that = this;

    util.request(api.UpdateWeixinMobile, {
        code: mobileCode
      }, 'POST')
      .then(res => {
        if (res.success) {
          let userInfo = app.globalData.userInfo;
          userInfo.phoneNumber = res.data;

          app.globalData.userInfo = userInfo;
          wx.setStorageSync('userInfo', JSON.stringify(userInfo));

          // 设置用户信息
          that.setData({
            userInfo: userInfo,
            showLoginDialog: false
          });

        } else {
          wx.showToast({
            title: '微信登录失败',
          })
        }
      })
      .catch((err) => {
        console.log(err)
      })

  },
  // 移到个人信息页面
  exitLogin: function () {
    let that = this;
    wx.showModal({
      title: '',
      confirmColor: '#e54d42',
      content: '退出登录？',
      success: function (res) {
        if (res.confirm) {

          wx.showToast({
            title: '退出成功！'
          });

          app.globalData.userInfo = {
            name: '',
            avatar: app.globalData.defaultAvatar
          };
          app.globalData.token = '';

          // wx.clearStorageSync(); 
          wx.removeStorageSync('token');
          wx.removeStorageSync('userInfo');

          that.setData({
            userInfo: {
              name: '',
              avatar: app.globalData.defaultAvatar
            }
          });

        } else {

          wx.showToast({
            title: '退出失败！'
          });
        }
      }
    })

  }

})