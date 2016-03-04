## 最新版obfs4 V1.0.0.0##

即将发布

##特性##

- 支持Squid技术
- 客户端使用obfs4混淆加密
- 基于Framework3.5框架开发
- 支持智能和全局!

## Server

### Install

Debian/Ubuntu

```
apt-get install gcc python-pip python-dev
pip install obfsproxy

```

### Prepare server port 
对squid进行混淆加密
```
/usr/local/bin/obfsproxy --data-dir=/tmp/scramblesuit-server scramblesuit --password=FANGBINXINGFUCKYOURMOTHERSASS444 --dest=127.0.0.1:25 server 0.0.0.0:8087

```
scramblesuit 是一种安全性稍高的加密工作方式，该方式工作时需要临时文件夹存放yaml ticket，故用 —data-dir 参数指定目录。—password 指定了加密密码，必须为 BASE32 字符，即大写字母加数字共32位的字符串。
—dest 指定目标端口，此处填写 squid 服务端口25。 server 为混淆后对外监听端口，0.0.0.0 表示允许所有网段地址连接。
运行成功后会提示


###  start automatically during the system startup (Debian)
nano /etc/rc.local 

```
Add the following content to /etc/rc.local 

(/usr/local/bin/obfsproxy --data-dir=/tmp/scramblesuit-server scramblesuit --password=FANGBINXINGFUCKYOURMOTHERSASS444 --dest=127.0.0.1:25 server 0.0.0.0:23333 >/dev/null 2>&1 &)

```
