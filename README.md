
## obfsproxy 1.0.0.0##


* obfsproxy  [下载](https://github.com/squidproxy/obfs4/releases/download/v1.0.0.0/obfsproxy.exe)

##特性##

- obfsproxy套件
- 支持启动和配置obfsproxy
- 全局代理
- 完美兼容finalspped、Squid
- 可以解决finalspeed断流的问题,提高服务器流量传输的稳定性和安全
- 基于Framework3.5框架开发


##obfsproxy介绍##
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

```
obfsproxy --data-dir ~/.obfs/ scramblesuit --dest 127.0.0.1:22 --password SBSB4444FANGBINXING4SBSBSBSBSBSB server 0.0.0.0:8080 &

```


###  start automatically during the system startup (Debian)
nano /etc/rc.local 

```
Add the following content to /etc/rc.local 

(obfsproxy scramblesuit --dest 1xx.xxx.xxx.xxx:8080 --password SBSB4444FANGBINXING4SBSBSBSBSBSB client 127.0.0.1:22222 >/dev/null 2>&1 &)

```
