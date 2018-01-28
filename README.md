# Website Password Changer

A KeePass plugin that automates password changes on websites, because we're all lazy.

-----

Passwords for important accounts should be changed once or twice a year;
and definitely whenever you suspect they're compromised.

Changing passwords can be tiresome, though. You have to,

1. Recall your current password
1. Log in to the site
1. Think of a new password
1. Change your password
1. Remember your new password

This gets harder when password requirements get complex, or long.

Website Password Changer (WPC) is a KeePass plugin that helps you do
all of the above, with a few simple clicks.

-----

### Usage (Windows)

After building,

1. Copy + Paste the following to your KeePass `Plugins` folder,
    1. `WPC.dll`
    1. `WebDriver.dll`
    1. `WebDriver.Support.dll`
    1. `YamlDotNet.dll`
1. Copy + Paste the `sites.yml` to the folder `KeePass.exe` is in.
1. Download https://chromedriver.storage.googleapis.com/2.35/chromedriver_win32.zip
1. Extract to the folder `KeePass.exe` is in.
1. Run

Support for additional sites may be added by modifying the `sites.yml` file.

Your directory should look like,

```
KeePass/
    + chromedriver/
        + chromedriver (Executable)
    + Plugins/
        + WPC.dll
        + WebDriver.dll
        + WebDriver.Support.dll
        + YamlDotNet.dll
    + sites.yml
    + KeePass.exe
```

### Usage (Others)

After building,

1. Copy + Paste the following to the folder `KeePass.exe` is in,
    1. `WPC.dll`
    1. `WebDriver.dll`
    1. `WebDriver.Support.dll`
    1. `YamlDotNet.dll`
1. Copy + Paste the `sites.yml` to the folder `KeePass.exe` is in.
1. Download http://chromedriver.storage.googleapis.com/2.35/chromedriver_linux64.zip
1. Extract to the folder `KeePass.exe` is in.
1. Run with `mono` (`mono ./KeePass.exe`)

Support for additional sites may be added by modifying the `sites.yml` file.

Your directory should look like,

```
KeePass/
    + chromedriver/
        + chromedriver (Executable)
    + WPC.dll
    + WebDriver.dll
    + WebDriver.Support.dll
    + YamlDotNet.dll
    + sites.yml
    + KeePass.exe
```

### How it Works

+ WPC uses KeePass to manage your passwords, and generate new ones.
+ WPC uses Selenium, to automate logging in, and changing passwords.
+ Macros are made for each website.
+ Each site's macro takes about 2-5 minutes to write.
+ A `.yml` file allows you to add your own password changing macros, for any website.

### Applications

WPC can keep your accounts safe, by making it painless to
change your passwords when you need to.

Examples of important accounts,

+ Banking
+ Social Media
+ Business
+ Online Journals

### Challenges

+ Barely any documentation for KeePass plugin development
+ Some websites are harder to write macros for

### What is KeePass?

Many people use the built-in password managers of Chrome, and Firefox, to
manage their list of sites, and passwords.

However, these password managers are simple; nothing more than data stores.

KeePass is a feature-rich, and highly customizable password manager;
for those that want more control over their security.

### Roadmap

+ Browser extensions to replace Selenium
+ Improve UI
+ Increase robustness
+ Automatically change password on expiration
+ Custom password generation options
