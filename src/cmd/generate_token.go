package cmd

import (
	"errors"

	"github.com/spf13/cobra"
)

var (
	generateTokenCmd = &cobra.Command{
		Use:   "generate-token",
		Short: "Generate an access token for the app installation",
		RunE:  RunGenerateToken,
	}
)

func RunGenerateToken(cmd *cobra.Command, args []string) error {
	return errors.New("Not implemented")
}
