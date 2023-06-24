import React, { Fragment } from 'react';
import { formatMessage } from 'umi/locale';
import Link from 'umi/link';
import { Icon } from 'antd';
import GlobalFooter from '@/components/GlobalFooter';
import SelectLang from '@/components/SelectLang';
import styles from './UserLayout.less';
import logo from '../assets/logo.png';

const links = [
  // {
  //   key: 'help',
  //   title: formatMessage({ id: 'layout.user.link.help' }),
  //   href: '',
  // },
  // {
  //   key: 'privacy',
  //   title: formatMessage({ id: 'layout.user.link.privacy' }),
  //   href: '',
  // },
  // {
  //   key: 'terms',
  //   title: formatMessage({ id: 'layout.user.link.terms' }),
  //   href: '',
  // },
  {
    key: 'ICP备案号',
    title: <span>豫ICP备2023013181号</span>,
    href: 'https://beian.miit.gov.cn/',
    blankTarget: true,
  },
];

const copyright = (
  <Fragment>
    Copyright <Icon type="copyright" /> 2023{' '}
    <a href="https://cairuimall.com/" target="_blank">
      CaiRuiMall.COM
    </a>
  </Fragment>
);

class UserLayout extends React.PureComponent {
  // @TODO title
  // getPageTitle() {
  //   const { routerData, location } = this.props;
  //   const { pathname } = location;
  //   let title = 'Ant Design Pro';
  //   if (routerData[pathname] && routerData[pathname].name) {
  //     title = `${routerData[pathname].name} - Ant Design Pro`;
  //   }
  //   return title;
  // }

  render() {
    const { children } = this.props;
    return (
      // @TODO <DocumentTitle title={this.getPageTitle()}>
      <div className={styles.container}>
        {/* <div className={styles.lang}>
          <SelectLang />
        </div> */}
        <div className={styles.content}>
          <div className={styles.top}>
            <div className={styles.header}>
              <Link to="/">
                <img alt="logo" className={styles.logo} src={logo} />
                <span className={styles.title}>菜蕊商城</span>
              </Link>
            </div>
            <div className={styles.desc}>
              {/* <p>菜蕊商城是一个基于.NET Core的电子商务系统。</p>
              <p>它是免费的、简单的、易于定制的。</p>
              <p>通过它的模块化架构，您可以轻松地为自己的业务开发自己的模块或在市场上安装模块。</p>
              <p>菜蕊商城可以部署在docker容器、Windows、Linux以及任何云提供商中。</p> */}
            </div>
          </div>
          {children}
        </div>
        <GlobalFooter links={links} copyright={copyright} />
      </div>
    );
  }
}

export default UserLayout;
