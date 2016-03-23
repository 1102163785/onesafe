
## obfsproxy 1.0.0.1##


* obfsproxy  [下载](https://github.com/squidproxy/obfs4/releases/download/v1.0.0.1/obfsproxydl.exe)

##特性##

- obfsproxy套件
- 支持启动和配置obfsproxy
- 全局代理
- 完美兼容finalspped、Squid
- 可以解决finalspeed断流的问题,提高服务器流量传输的稳定性和安全
- 基于Framework3.5框架开发


客户端功能介绍:
obfsproxy鱿鱼版既可以作为obfsproxy客户端使用,也可以配合finalspeed对其流量做二次加密!

##使用说明##

- BASE32  scramblesuit 是一种安全性稍高的加密工作方式, scramblesuit 通过自定义的 BASE32字符串混淆流量,因为BASE32 是用户可以自定义的
这使得第三方很难模拟obfsproxy客户端!

- 远程服务器端口 这个输入finalspeed客户端的本地端口或者填写你的obfsproxy服务器远程端口

- 远程服务器地址 填写finalspeed本地监听地址比如:127.0.0.1,也可以填写你的obfsproxy服务器的IP地址

- 可以自定义任何端口,在你启动obfsproxy的时候,客户端将设置IE的系统代理为该端口

- 本地监听地址 一般默认为127.0.0.1,这个是obfsproxy客户端本地监听地址

```
注意事项: 右侧监听状态栏功能: 提供 finalspeed的本地端口或者obfsproxy的远程端口状态分析.如果obfsproxy启动成功，本地端口监听端口将
显示监听成!如果监听失败,确保obfsproxy解压在一个英文路径的目录不留空格!中文会遇到问题!后续会解决这个bug!﻿

```

##obfsproxy介绍##

obfsproxy 可以对任意的流量做混淆处理,比如SS、squid、Openvpn等产生的流量
obfsproxy is a tool that attempts to circumvent censorship, by transforming the Tor traffic between the client and the bridge. 
This way, censors, who usually monitor traffic between the client and the bridge,
 will see innocent-looking transformed traffic instead of the actual Tor traffic.


##feature##

- C# architecture
- Base on Framework3.5
- Support Smart and global Mode 


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

### 客户端使用说明###

* 直接使用我们设计的obfsproxy客户端,他集成了设置和启动、代理等功能一体,让整个obfsproxy 更简单上手.
* V1.0.0.1 [下载](https://github.com/squidproxy/obfs4/releases/download/v1.0.0.1/obfsproxydl.exe)

- BASE32  scramblesuit 是一种安全性稍高的加密工作方式, scramblesuit 通过自定义的 BASE32字符串混淆流量,因为BASE32 是用户可以自定义的
这使得第三方很难模拟obfsproxy客户端!

- 远程服务器端口 这个输入finalspeed客户端的本地端口或者填写你的obfsproxy服务器远程端口

- 远程服务器地址 填写finalspeed本地监听地址比如:127.0.0.1,也可以填写你的obfsproxy服务器的IP地址

- 可以自定义任何端口,在你启动obfsproxy的时候,客户端将设置IE的系统代理为该端口

- 本地监听地址 一般默认为127.0.0.1,这个是obfsproxy客户端本地监听地址

```
注意事项: 右侧监听状态栏功能: 提供 finalspeed的本地端口或者obfsproxy的远程端口状态分析.如果obfsproxy启动成功，本地端口监听端口将
显示监听成!如果监听失败,确保obfsproxy解压在一个英文路径的目录不留空格!中文会遇到问题!后续会解决这个bug!﻿

```