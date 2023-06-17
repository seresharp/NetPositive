# NetPositive
Tool for mass scanning .Net Assembly files based on Signatures.

## Usage
    Usage: NetPositive [-r] -t target [--allowPathEscape] [-S scanSpec] [-O outputDir]

        -r                      Recursively enumerate target directories
        -t                      Target to scan, can be supplied multiple times and be a file or directory
        --allowPathEscape       Allow the file enumeration to access directories outside supplied target paths
        -S                      Scan Specification, for example: GenericFileMethodScanner:MetaDeserialization
        -O                      Output directory to write results to.

## Signatures

Please see [the definitions README](NetPositive/Definitions/Readme.md) for a more complete description of how to define signatures.