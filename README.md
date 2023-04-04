# aprongen
 
## Description
`aprongen` is a tool to take `aeroway=apron` tagged OSM data, then convert and add them as `<Apron>` elements in a Microsoft Flight Simulator scenery XML file.

## Getting Started

`aprongen` requires two existing files:

- An OSM file containing `aeroway=apron` tagged data
- An MSFS scenery XML file containing the `<Airport>` and `<Apron />` elements.

## Requirements

- [.NET 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)

## Usage

    aprongen.exe [OSM file] [Scenery XML file]

## Example

    aprongen.exe rickenbacker.osm KLCK.xml

## To-Do

- `[ ]` Add support for downloading OSM data from the OpenStreetMap API.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Contributing

Feel free to poke around and submit a pull request if you'd like to contribute. If you see any issues, don't hesitate to open an issue.

## Acknowledgments

- [OpenStreetMap](https://www.openstreetmap.org)

## Disclaimer

This project is not affiliated with or endorsed by Microsoft or Flight Simulator in any way. Microsoft and Flight Simulator are trademarks of Microsoft Corporation in the United States and/or other countries. All other trademarks are the property of their respective owners. Use at your own risk.