var util = require('../../utils/util.js');
var api = require('../../config/api.js');

Page({
  data: {
    categories: [],
    current: {},
    name: "",
    subId: 0,
    predicate: 'id',
    reverse: true,
    pageNum: 1,
    pageSize: 10,
    total: 0,
    pageData: []
  },
  onLoad: function (options) {
    this.getCatalog();
  },
  onReady: function () {
    // 页面渲染完成
  },
  onShow: function () {
    // 页面显示
    // this.getCatalog();
  },
  onHide: function () {
    // 页面隐藏
  },
  onUnload: function () {
    // 页面关闭
  },
  onReachBottom: function () {
    // 当界面的下方距离页面底部距离小于100像素时触发回调
    if (this.data.total > 0 && this.data.pageNum * this.data.pageSize < this.data.total) {
      this.setData({
        pageNum: this.data.pageNum + 1
      }, () => {
        this.getGoods();
      });
    }
  },
  getCatalog() {
    wx.showLoading({
      title: '加载中...'
    });
    let that = this;
    util.request(api.Catalogs)
      .then(function (res) {
        wx.hideLoading();
        if (res.success === true) {
          var curr = (res.data && res.data.length > 0) ? res.data[0] : {};
          var subId = curr.subCategories && curr.subCategories.length > 0 ? curr.subCategories[0].id : 0;

          that.setData({
            categories: res.data,
            current: curr,
            subId: subId
          }, () => {
            that.getGoods();
          });
        }
      });
  },
  switchCate(e) {
    let id = e.currentTarget.dataset.id;
    if (this.data.current.id == id) {
      return false;
    }
    let first = this.data.categories.find(c => c.id == id);
    let firstSub = first.subCategories?.[0];
    this.setData({
      current: first,
      subId: firstSub?.id,
      pageData: [],
      pageNum: 1,
      total: 0
    }, () => {
      this.getGoods();
    });
  },
  switchSubCate: function (event) {

    let id = event.currentTarget.dataset.id;
    if (this.data.subId == id) {
      return false;
    }
    let subCategories = this.data.categories.find(c => c.id == this.data.current.id)?.subCategories ?? [];
    let first = subCategories.find(c => c.id == id);

    if (first) {
      this.setData({
        pageData: [],
        pageNum: 1,
        total: 0,
        subId: first.id
      }, () => {
        this.getGoods();
      });
    }
  },
  searchChange: function (e) {
    let pName = e.detail.value;
    if (pName) {
      this.setData({
        name: pName
      });
    }
  },
  getGoods: function () {
    let that = this;
    var params = {
      pagination: {
        current: that.data.pageNum,
        pageSize: that.data.pageSize
      },
      sort: {
        predicate: that.data.predicate,
        reverse: that.data.reverse,
      },
      search: {
        name: that.data.name,
        categoryId: that.data.subId,
      }
    };
    util.request(api.GoodsGrid, params, "POST")
      .then(function (res) {
        if (res.success === true) {
          let origin_data = that.data.pageData || [];
          let new_data = origin_data.concat(res.data.list)
          that.setData({
            pageData: new_data,
            total: parseInt(res.data.pagination.total)
          });
        }
      });
  },
  navToGoods: function (e) {
    wx.navigateTo({
      url: `../goods/goods?id=${e.currentTarget.dataset.id}`
    })
  }
})