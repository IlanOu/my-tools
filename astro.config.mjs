// @ts-check
import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';
import starlightAutoSidebar from 'starlight-auto-sidebar'
import catppuccin from "@catppuccin/starlight";

// https://astro.build/config
export default defineConfig({
	integrations: [
		starlight({
			title: 'My Tools',
			social: [{ icon: 'github', label: 'GitHub', href: 'https://github.com/ilanou/my-tools' }],
			// favicon: 'public/favicon.ico',
			plugins: [catppuccin({
				dark: { flavor: "mocha", accent: "pink" },
				light: { flavor: "latte", accent: "sky" }
			}), starlightAutoSidebar()],
			sidebar: [
				{
					label: 'Guides',
					items: [
						{ label: 'Accueil', slug: 'guides/main' },
					],
				},
				{
					label: 'Unity',
					autogenerate: { directory: 'unity' },
				},
			],
		}),
	],
});
