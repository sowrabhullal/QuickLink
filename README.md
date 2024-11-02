# QuickLink - URL Shortener REST API

QuickLink is a URL shortener REST API developed using ASP.NET and Redis, designed to provide a simple and efficient way to shorten URLs. This project implements Base62 encoding for generating unique short codes, ensuring fast and reliable URL management.

## Features

- **URL Shortening**: Convert long URLs into short, manageable links.
- **Base62 Encoding**: Utilizes Base62 encoding for generating unique short codes, ensuring a compact representation.
- **Atomic Counter Management**: Handles concurrent requests effectively, ensuring data integrity and preventing collisions.
- **Efficient Storage and Retrieval**: Optimizes the process of storing and retrieving original URLs for improved performance.

## Technologies Used

- **ASP.NET**: Framework for building the REST API.
- **Redis**: In-memory data structure store used for fast data access and storage.
- **Base62 Encoding**: Algorithm for generating unique short codes from URLs.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or higher)
- [Redis](https://redis.io/download) (make sure it's running)

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/sowrabhullal/QuickLink.git
   cd QuickLink
