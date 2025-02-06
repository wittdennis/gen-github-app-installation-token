package cmd

import (
	"fmt"

	"github.com/spf13/cobra"
)

var (
	rootCmd = &cobra.Command{
		Use:   "github-app-installation",
		Short: "github-app-installation generates a token for a github app installation",
	}
)

func Execute() {
	if err := rootCmd.Execute(); err != nil {
		fmt.Println("Failed to execute root command")
	}
}

func init() {
	rootCmd.AddCommand(generateTokenCmd)
}
