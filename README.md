# HyPlayer.NeteaseProvider

[![NuGet Publish](https://github.com/HyPlayer/HyPlayer.NeteaseProvider/actions/workflows/nuget-push.yml/badge.svg)](https://github.com/HyPlayer/HyPlayer.NeteaseProvider/actions/workflows/nuget-push.yml)
[![License](https://img.shields.io/github/license/HyPlayer/HyPlayer.NeteaseProvider)](LICENSE)

A comprehensive .NET provider for Netease Cloud Music API integration with HyPlayer ecosystem.

## Overview

HyPlayer.NeteaseProvider is a modern, type-safe provider library for integrating Netease Cloud Music services with the HyPlayer music player framework. It consists of two main components:

- **HyPlayer.NeteaseApi** - Low-level API wrapper for Netease Cloud Music endpoints
- **HyPlayer.NeteaseProvider** - High-level provider implementation for HyPlayer integration

## Features

- Multi-target support: `.NET Standard 2.0` and `.NET 9.0`
- Type-safe API contracts with automatic JSON serialization
- AOT (Ahead-of-Time) compatible for .NET 9.0+
- Async/await support throughout
- Comprehensive Netease API coverage
- Integration with HyPlayer.PlayCore abstraction layer
- Entity caching with Depository abstraction


## Getting Started

### Installation

Install the NuGet package:

` dotnet add package HyPlayer.NeteaseProvider `

This will automatically include the `HyPlayer.NeteaseApi` dependency.

### Prerequisites

- `.NET Standard 2.0` or higher
- `.NET 9.0` or higher (for latest features and AOT support)

## Components

### HyPlayer.NeteaseApi (v0.1.0)

Low-level API wrapper providing direct access to Netease Cloud Music endpoints.

**Key Features:**
- RESTful API contract definitions
- Automatic JSON serialization with source-generated serialization context
- Support for multiple authentication methods
- Comprehensive error handling

**Dependencies:**
- System.Text.Json v10.0.1

### HyPlayer.NeteaseProvider (v0.0.11)

High-level provider implementing HyPlayer provider abstraction for seamless integration.

**Key Features:**
- HyPlayer.PlayCore abstraction implementation
- Entity repository pattern with Depository.Abstraction
- Radio channel APIs support
- Song, playlist, and album management

**Dependencies:**
- HyPlayer.PlayCore.Abstraction v0.1.4
- Depository.Abstraction v3.2.0
- HyPlayer.NeteaseApi

## Building

Requires `.NET SDK 10.0.0` or later .

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the [License](LICENSE) - see the LICENSE file for details.

## Related Projects

- [HyPlayer](https://github.com/HyPlayer/HyPlayer) - Main music player application
- [HyPlayer.PlayCore](https://github.com/HyPlayer/HyPlayer.PlayCore) - Core playback abstraction
- [Depository](https://github.com/HyPlayer/Depository) - Entity repository framework

## Support

For issues, feature requests, or questions, please visit the [GitHub Issues](https://github.com/HyPlayer/HyPlayer.NeteaseProvider/issues) page.

---