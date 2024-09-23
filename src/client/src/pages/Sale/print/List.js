import React, { PureComponent, Fragment } from 'react';
import { connect } from 'dva';
import moment from 'moment';
import {
  List,
  Card,
  Input,
  Button,
  Modal,
  Form,
  notification,
  Table,
  Popconfirm,
  Divider,
  Select,
  Tag,
  Icon,
  Redio,
  Menu,
  Dropdown,
  Switch,
  Row,
  Col,
  InputNumber,
  DatePicker,
  Checkbox,
  Spin,
} from 'antd';

import PageHeaderWrapper from '@/components/PageHeaderWrapper';
import StandardTable from '@/components/StandardTable';

import router from 'umi/router';
import Link from 'umi/link';

import styles from './List.less';

const { RangePicker } = DatePicker;
const FormItem = Form.Item;
const Option = Select.Option;
const ShippingStatus = [
  { key: 0, value: '无需配送', color: '' },
  { key: 20, value: '未发货', color: '#f50' },
  { key: 25, value: '部分发货', color: '#2db7f5' },
  { key: 30, value: '已发货', color: '#108ee9' },
  { key: 40, value: '已收货', color: '#87d068' },
];

const OrderStatus = [
  { key: 0, value: '新订单', color: 'purple' },
  { key: 10, value: '挂起', color: 'red' },
  { key: 20, value: '待付款', color: 'orange' },
  { key: 25, value: '付款失败', color: 'red' },
  { key: 30, value: '已付款', color: 'lime' },
  { key: 40, value: '发货中', color: 'cyan' },
  { key: 50, value: '已发货', color: 'blue' },
  { key: 60, value: '交易成功', color: 'green' },
  { key: 70, value: '交易取消', color: '' },
];

const CancelOrderStatus = [0, 20, 25];
const DeleteOrderStatus = [60, 70];
const OnHoldNotOrderStatus = [10, 60, 70];
const NotMoreStatus = [10];
const DeliveryOrderStatus = [30, 40];
const DeliveryShippingStatus = [null, 0, 20, 25];

@connect()
@Form.create()
class OrderList extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,

      pageNum: 1,
      pageSize: 10,
      predicate: '',
      reverse: true,
      pageData: {
        list: [],
        pagination: {},
      },

      // expandForm: false,
      queryParam: {},

      users: [],
      usersLoading: false,

      onHoldReason: '',
      cancelReason: '',
    };
  }

  columns = [
    {
      title: '操作',
      key: 'operation',
      fixed: 'left',
      align: 'center',
      width: 135,
      render: (text, record) => (
        <Fragment>
          <Button.Group>
            <Button icon="eye" size="small" onClick={() => this.handleDetail(record.id)} />
            <Button icon="edit" size="small" onClick={() => this.handleEdit(record.id)} />
            <Dropdown
              overlay={
                <Menu>
                  {[0, 20, 25].indexOf(record.orderStatus) >= 0 ? (
                    <Menu.Item>
                      <a
                        onClick={() => {
                          Modal.confirm({
                            title: '标记付款：' + record.no,
                            content: '确定对此订单标记付款吗？',
                            okText: '确认',
                            cancelText: '取消',
                            onOk: () => this.paymentItem(record.id),
                          });
                        }}
                      >
                        标记付款
                      </a>
                    </Menu.Item>
                  ) : null}
                  {DeliveryOrderStatus.indexOf(record.orderStatus) >= 0 &&
                  DeliveryShippingStatus.indexOf(record.shippingStatus) >= 0 ? (
                    <Menu.Item>
                      <a
                        onClick={() => {
                          this.handleDelivery(record.no);
                        }}
                      >
                        发货
                      </a>
                    </Menu.Item>
                  ) : null}
                  {CancelOrderStatus.indexOf(record.orderStatus) >= 0 ? (
                    <Menu.Item>
                      <a
                        onClick={() => {
                          this.setState({ cancelReason: '' });
                          Modal.confirm({
                            title: '确定取消订单吗？',
                            content: (
                              <Input.TextArea
                                onChange={e => {
                                  this.setState({ cancelReason: e.target.value });
                                }}
                                placeholder="取消原因"
                                rows={3}
                              />
                            ),
                            okText: '确认',
                            cancelText: '取消',
                            onOk: () => this.cancelItem(record.id),
                          });
                        }}
                      >
                        取消订单
                      </a>
                    </Menu.Item>
                  ) : null}
                  {DeleteOrderStatus.indexOf(record.orderStatus) >= 0 ? (
                    <Menu.Item>
                      <a
                        onClick={() => {
                          Modal.confirm({
                            title: '删除订单',
                            content: '确定删除订单吗？',
                            okText: '确认',
                            cancelText: '取消',
                            onOk: () => this.deleteItem(record.id),
                          });
                        }}
                      >
                        删除订单
                      </a>
                    </Menu.Item>
                  ) : null}
                  {OnHoldNotOrderStatus.indexOf(record.orderStatus) < 0 ? (
                    <Menu.Item>
                      <a
                        onClick={() => {
                          Modal.confirm({
                            title: '挂起订单',
                            content: '确定挂起订单吗？',
                            okText: '确认',
                            cancelText: '取消',
                            onOk: () => this.onHoldItem(record.id),
                          });
                        }}
                      >
                        挂起订单
                      </a>
                    </Menu.Item>
                  ) : null}
                </Menu>
              }
            >
              <Button icon="ellipsis" size="small" />
            </Dropdown>
          </Button.Group>
        </Fragment>
      ),
    },
    // {
    //     title: 'ID',
    //     dataIndex: 'id',
    //     // fixed: 'left',
    //     sorter: true,
    //     defaultSortOrder: 'descend',
    //     width: 100,
    // },
    {
      title: '订单编号',
      dataIndex: 'no',
      // fixed: 'left',
      sorter: true,
      // defaultSortOrder: 'descend',
      width: 180,
    },
    {
      title: '订单状态',
      dataIndex: 'orderStatus',
      sorter: true,
      width: 120,
      render: val => {
        if (val || val === 0) {
          let first = OrderStatus.find(c => c.key == val);
          if (first) {
            return <Tag color={first.color}>{first.value}</Tag>;
          }
        }
        return <Tag>无</Tag>;
      },
    },
    {
      title: '配送状态',
      dataIndex: 'shippingStatus',
      sorter: true,
      width: 120,
      render: val => {
        if (val || val === 0) {
          let first = ShippingStatus.find(c => c.key == val);
          if (first) {
            return <Tag color={first.color}>{first.value}</Tag>;
          }
        }
        return '-';
      },
    },
    {
      title: '订单总额',
      dataIndex: 'orderTotal',
      sorter: true,
      width: 120,
    },
    {
      title: '客户',
      dataIndex: 'customerName',
      sorter: true,
    },
    {
      title: '创建时间',
      dataIndex: 'createdOn',
      sorter: true,
      width: 120,
      render: val => <span>{moment(val).format('YYYY-MM-DD')}</span>,
    },
    // {
    //     title: '更新时间',
    //     dataIndex: 'updatedOn',
    //     sorter: true,
    //     width: 120,
    //     render: val => <span>{moment(val).format('YYYY-MM-DD')}</span>,
    // }
  ];

  componentDidMount() {
    this.queryData(this.state.queryParam);
  }

  handleQueryUsers = nameOrPhone => {
    const { dispatch } = this.props;
    this.setState({ usersLoading: true });
    new Promise(resolve => {
      dispatch({
        type: 'system/users',
        payload: {
          resolve,
          params: { nameOrPhone },
        },
      });
    }).then(res => {
      this.setState({ usersLoading: false });
      if (res.success === true) {
        this.setState({ users: res.data });
      } else {
        notification.error({ message: res.message });
      }
    });
  };

  queryDataFirst = () => {
    this.setState(
      {
        pageNum: 1,
      },
      () => {
        this.queryData();
      }
    );
  };

  queryData = () => {
    this.setState({ loading: true });
    const { dispatch } = this.props;

    let search = this.state.queryParam;
    // todo set order status
    search.orderStatus = [20];
    this.setState({ queryParam: search });

    let params = {
      search: search,
      pagination: {
        current: this.state.pageNum,
        pageSize: this.state.pageSize,
      },
      sort: {
        predicate: this.state.predicate,
        reverse: this.state.reverse,
      },
    };

    new Promise(resolve => {
      dispatch({
        type: 'order/grid',
        payload: {
          resolve,
          params,
        },
      });
    }).then(res => {
      this.setState({ loading: false });
      if (res.success === true) {
        this.setState({
          pageData: res.data,
        });
      } else {
        notification.error({
          message: res.message,
        });
      }
    });
  };

  handleSearch = e => {
    let search = {
      orderStatus:[20],
    };

    this.setState({ queryParam: search }, () => {
      this.queryData();
    });
  };

  handleStandardTableChange = (pagination, filtersArg, sorter) => {
    var firstPage = this.state.predicate && sorter.field != this.state.predicate;
    this.setState(
      {
        pageNum: pagination.current,
        pageSize: pagination.pageSize,
      },
      () => {
        if (sorter.field) {
          this.setState(
            {
              predicate: sorter.field,
              reverse: sorter.order == 'descend',
            },
            () => {
              if (firstPage) this.queryDataFirst();
              else this.queryData();
            }
          );
        } else {
          if (firstPage) this.queryDataFirst();
          else this.queryData();
        }
      }
    );
  };

  handleDetail = id => {
    router.push({
      pathname: '../Order/detail',
      query: {
        id: id,
      },
    });
  };

  handleEdit = id => {
    router.push({
      pathname: '../Order/edit',
      query: {
        id: id,
      },
    });
  };

  cancelItem = id => {
    this.setState({ loading: true });
    const { dispatch } = this.props;
    const params = { id, reason: this.state.cancelReason };
    new Promise(resolve => {
      dispatch({
        type: 'order/cancel',
        payload: {
          resolve,
          params,
        },
      });
    }).then(res => {
      this.setState({ loading: false });
      if (res.success === true) {
        this.queryData();
      } else {
        notification.error({ message: res.message });
      }
    });
  };

  onHoldItem = id => {
    this.setState({ loading: true });
    const { dispatch } = this.props;
    const params = { id };
    new Promise(resolve => {
      dispatch({
        type: 'order/onHold',
        payload: {
          resolve,
          params,
        },
      });
    }).then(res => {
      this.setState({ loading: false });
      if (res.success === true) {
        this.queryData();
      } else {
        notification.error({ message: res.message });
      }
    });
  };

  render() {
    const pagination = {
      showQuickJumper: true,
      showSizeChanger: true,
      pageSizeOptions: ['5', '10', '50', '100'],
      defaultPageSize: this.state.pageSize,
      defaultCurrent: this.state.pageNum,
      current: this.state.pageNum,
      pageSize: this.state.pageSize,
      total: this.state.pageData.pagination.total || 0,
      showTotal: (total, range) => {
        return `${range[0]}-${range[1]} 条 , 共 ${total} 条`;
      },
    };
    const action = (
      <Fragment>
        <Button onClick={this.handleAdd} type="primary" icon="printer">
          打印采购单
        </Button>
      </Fragment>
    );
    return (
      <PageHeaderWrapper title="待付款订单" action={action}>
        <div>
          <Card bordered={false}>
            <StandardTable
              pagination={pagination}
              loading={this.state.loading}
              data={this.state.pageData}
              rowKey={record => record.id}
              columns={this.columns}
              bordered
              onChange={this.handleStandardTableChange}
              scroll={{ x: 960 }}
            />
          </Card>
        </div>
      </PageHeaderWrapper>
    );
  }
}

export default OrderList;