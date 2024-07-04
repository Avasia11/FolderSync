# FolderSync

FolderSync is a C# program that synchronizes the contents of two folders: a source folder and a replica folder. The program maintains an identical copy of the source folder in the replica folder.

## Features

- One-way synchronization: The content of the replica folder is modified to exactly match the content of the source folder.
- Periodic synchronization.
- Logging of file creation/copy/removal operations to a log file and console output.
- Program parameters (folder paths, synchronization interval, log file path) are provided via command line arguments.

## Requirements

- .NET SDK

## Installation

1. Clone this repository:
    ```sh
    git clone https://github.com/yourusername/foldersync.git
    ```
2. Navigate to the project directory:
    ```sh
    cd foldersync
    ```
3. Open the project in Visual Studio or Visual Studio Code.

## Usage

1. Ensure you have two folders to use for testing: a source folder and a replica folder.
2. Create an empty log file, for example `log.txt`.

### Command Line Arguments

The program accepts four arguments:
1. Path to the source folder
2. Path to the replica folder
3. Interval in seconds between synchronizations
4. Path to the log file

### Usage Examples

To run the program, use the following command in the command line or configure the arguments in your development environment:

```sh
dotnet run "C:\Users\YourUsername\Desktop\SourceFolder" "C:\Users\YourUsername\Desktop\ReplicaFolder" 10 "C:\Users\YourUsername\Desktop\log.txt"
