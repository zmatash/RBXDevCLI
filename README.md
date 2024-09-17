# RBXDevCLI

This is a custom CLI tool for my personal workflow. Primarily designed for use with roblox-ts in a multi-place typescript monorepo.


## Currently implemented commands:
### zap
#### Usage
```ps 
RobloxDevCli zap --pathToZapConfig
```

Wraps the Redblox Zap networking tool and implments custom behaviour.
- Runs Zap to generate networking code.
- Modifies the generated type definition files with the following:
  - Wraps with and exports the Zap namespace.
  - Exports only types that end with 'Packet' to avoid unnecessary pollution.
- Runs ESLint over the generated type files to conform to the eslintrc in the project root. 