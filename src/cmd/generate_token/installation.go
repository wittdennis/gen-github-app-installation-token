package generate_token

import (
	"fmt"
	"github-app/config"
	"strconv"

	"github.com/jferrl/go-githubauth"
	"github.com/spf13/cobra"
)

var installationCmd = &cobra.Command{
	Use:   "installation",
	Short: "Generates a installation token for a specific installation of the app",
	Args: func(cmd *cobra.Command, args []string) error {
		if err := cobra.ExactArgs(1)(cmd, args); err != nil {
			return fmt.Errorf("Missing required argument installation-id")
		}

		if _, err := strconv.ParseInt(args[0], 10, 64); err != nil {
			return fmt.Errorf("Can't parse %s as valid installation id. Has to be int64 id", args[0])
		}

		return nil
	},
	RunE:         runInstallationTokenCmd,
	SilenceUsage: true,
}

func runInstallationTokenCmd(cmd *cobra.Command, args []string) error {
	installationId, _ := strconv.ParseInt(args[0], 10, 64)

	appTokenSource, err := githubauth.NewApplicationTokenSource(config.ApplicationId, config.PrivateKey)
	if err != nil {
		return err
	}

	installationTokenSource := githubauth.NewInstallationTokenSource(installationId, appTokenSource)
	token, err := installationTokenSource.Token()
	if err != nil {
		return err
	}

	fmt.Println(token.AccessToken)
	return nil
}
