package cmd

import (
	"fmt"
	"github-app/cmd/generate_token"
	"github-app/config"
	"os"
	"strconv"
	"strings"

	"github.com/spf13/cobra"
	"github.com/spf13/viper"
)

var (
	cfgFile string

	rootCmd = &cobra.Command{
		Use:   "github-app",
		Short: "github-app is a cli tool to interact with github apps",
	}
)

func Execute() {
	if err := rootCmd.Execute(); err != nil {
		os.Exit(1)
	}
}

func init() {
	cobra.OnInitialize(initConfig)

	rootCmd.PersistentFlags().StringVar(&cfgFile, "config", "", "config file (default is $HOME/.github-app/config.yaml)")

	rootCmd.AddCommand(generate_token.GenerateTokenCmd)
}

func initConfig() {
	if cfgFile != "" {
		viper.SetConfigFile(cfgFile)
	} else {
		home, err := os.UserHomeDir()
		cobra.CheckErr(err)

		viper.AddConfigPath(strings.Join([]string{home, ".github-app"}, "/"))
		viper.SetConfigName("config")
	}

	viper.AutomaticEnv()
	if err := viper.ReadInConfig(); err != nil {
		cobra.CheckErr(err)
	}

	if !viper.IsSet("appId") {
		cobra.CheckErr(fmt.Errorf("Missing required 'appId' in config"))
	}

	appId, err := strconv.ParseInt(viper.GetString("appId"), 10, 64)
	if err != nil {
		cobra.CheckErr(fmt.Errorf("Can't parse 'appId' into int64 id."))
	}
	config.ApplicationId = appId

	if !viper.IsSet("privateKey") {
		cobra.CheckErr(fmt.Errorf("Missing required 'privateKey' in config"))
	}
	privateKey := viper.GetString("privateKey")
	config.PrivateKey = []byte(privateKey)
}
