# RepositorySimples
仓储模式示例

前端模板采用 [AdminLTE](https://github.com/almasaeed2010/AdminLTE)

表格控件[bootstrap-table](https://github.com/wenzhixin/bootstrap-table)

数据库采用MySQL

结构说明

### 1. Infrastructure 基础设施层 

提供系统中与业务无关的基础设施功能

1. Infrastructure 工具组件，提供通用辅助操作，扩展方法，异常定义，日志记录定义与实现等功能
2. Infrastructure.Data 数据组件，提供与业务无关的EF数据上下文，单元操作，仓储操作。

### 2. Domain 领域模型

定义用于系统核心业务的实现的数据模型。

### 3. Persistence 业务数据访问

提供与业务相关的数据访问功能实体映射，数据迁移，仓储操作。

### 4. Service 后台服务

提供例如定时任务等功能。

### 5. Presentation 

1. RepositorySample.Service 数据服务，核心业务实现。
2. RepositorySample.Site.Models 网站业务视图模型。
3. RepositorySample.Site 网站业务实现，职能包含：业务执行权限检查，网站业务实体模型和核心业务实体模型转换，http相关数据处理。
4. RepositorySample UI展现，对action执行权限检查，接收用户输入并传递给业务层。


 参考： 
 
 [郭明峰](http://www.cnblogs.com/guomingfeng/archive/2013/05/19/mvc-overall-design.html)

 [WYRMS](https://github.com/wuyi23/WYRMS)

 [OpenAuth.Net](https://gitee.com/yubaolee/OpenAuth.Net)

 [Asp.Net Identity](https://github.com/aspnet/Identity)