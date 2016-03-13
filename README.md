# IdleLandsRedux
IdleLands being rewritten

# Setup

See https://github.com/IdleLands/IdleLandsRedux/wiki/Setup-guide-for-windows for windows guide. Also usable for linux, but use the xamarin repository instead.

# Starting order

1. run `git submodule init` to clone the dependencies
2. Configure both the webservice and IdleLandsRedux through app.config.
3. Start the webservice (this has the message queues that the Core server needs to connect to)
4. Start IdleLandsRedux
