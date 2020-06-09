if exists (select * from sysobjects where name='MsgOfModel')
drop table MsgOfModel
go
create table MsgOfModel
(
    id int identity primary key, -- ID
    titleOfModel varchar(50) not null, -- 模块名称
    classNameOfModel varchar(50) not null, -- 模块实例名
    nameOfMainTable varchar(50) not null, -- 主档表名
    nameOfDetailTable varchar(50) default null, -- 从档表名
    typeOfModel bit default '0', -- 模块样式，1 为主从档，0 为主档
    relationField text default null, -- 主从关联
    create_time datetime not null, --创建时间',
    update_time datetime not null --更新时间',
)

if exists (select * from sysobjects where name='PropertyOfColumn')
drop table PropertyOfColumn
go
create table PropertyOfColumn
(
    id int identity primary key, -- ID
    idOfModel int not null, -- 模型 ID
    nameOfTable varchar(50) not null, -- 表名
    title varchar(50) default null, -- 对应表中字段的标题显示文本
    field varchar(50) not null, -- 表中字段的名称
    width int default null, -- 用于显示字段时的宽度
    isFreezed bit default '0', -- 是否冻结
    formula text default null, -- 公式
    defaultValue text default null, -- 默认值
    isReadOnly bit default '0', -- 是否只读
    isShow bit default '1', -- 是否显示
    create_time datetime not null, --创建时间',
    update_time datetime not null --更新时间',
)