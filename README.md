
## OneSafe 1.7##


* OneSafe [下载](https://github.com/squidproxy/obfs4/releases/)

##特性##

- 客户端 支持obfs4 模式

- 支持pac列表智能技术,可以绕过国内域名(最新加入特性)

- 支持全局,任意视频安全浏览

-  专门针对部署了squid+obfs4 的服务器

-  操作简单速度看1080P已无问题


```
##使用说明##


##OneSafe使用的技术介绍##

obfsproxy 可以对任意的流量做混淆处理,比如SS、squid、Openvpn等产生的流量
obfsproxy is a tool that attempts to circumvent censorship, by transforming the Tor traffic between the client and the bridge. 
This way, censors, who usually monitor traffic between the client and the bridge,
 will see innocent-looking transformed traffic instead of the actual Tor traffic.

```

## Server

### Install

Debian/Ubuntu

```
apt-get install gcc python-pip python-dev
pip install obfsproxy

```

### Prepare server port 

一个命令完成对特定端口的混淆,比如下面是对squid 25做混淆, 同时启动一个8080 给obfsproxy客户端 使用! 
```
obfsproxy --data-dir ~/.obfs/ scramblesuit --dest 101.101.101.101:25 --password SBSB4444FANGBINXING4SBSBSBSBSBSB server 0.0.0.0:8080 &

```


###  start automatically during the system startup (Debian)
nano /etc/rc.local 

```
Add the following content to /etc/rc.local 

(obfsproxy scramblesuit --dest 101.101.101.101:8080 --password SBSB4444FANGBINXING4SBSBSBSBSBSB client 127.0.0.1:22222 >/dev/null 2>&1 &)

```

### 源码遵循协议###

## License

Copyright (C) 2016 Max Lv <max.c.lv@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see <http://www.gnu.org/licenses/>.