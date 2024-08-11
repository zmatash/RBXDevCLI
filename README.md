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
- Converts the generated type files into modules to avoid a problem with globally available types.
- Runs ESLint over the generated type files to conform to the eslintrc in the project root. 