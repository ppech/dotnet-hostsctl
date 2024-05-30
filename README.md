# `hostsctl`

`hostsctl` is a command-line tool for managing your system's hosts file. It provides commands for:
- list entries
- exists of entry
- add, remove, enable, and disable entries
- backup and restore hosts file

With options you can specify:
- specify a custom input/output file
- use JSON output format

## Table of Contents

- [Features](#features)
- [Commands](#commands)
- [Options](#options)
- [Installation](#installation)
- [Build with](#build-with)
- [License](#license)

## Commands

* `list` list the entries in the hosts file
* `backup` backups the hosts file
* `restore` restores the hosts file from a backup
* `add <hostname>` adds entry to the hosts file
* `remove <hostname>` removes entry from hosts file
* `enable <hostname>` enables entry in hosts file
* `disable <hostname>` disables entry in hosts file
* `exists <hostname>` checks if entry exists in hosts file
* `open` opens hosts file (using shell execute)

## Options

* `-i|--input <file>`: path of input file, default value depends on operating system
* `-o|--output <file>`: path of output file, default value is same as input file
* `-j|--json`: output as JSON
* `<hostname>`: host name, ex. app.mydomain.local
* `[ip]`: ip address, default is 127.0.0.1

## Installation

You can install it using the `dotnet tool install ppech.dotnet.hostsctl --global` command.

To update `dotnet.hostsctl` to the latest version, use the `dotnet tool update` command.

## Build with

* https://github.com/spectreconsole/spectre.console

## License

This project is licensed under the [MIT License](LICENSE).
